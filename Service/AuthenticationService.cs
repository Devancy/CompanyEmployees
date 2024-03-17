using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using Shared.DataTransferObjects;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Service;
public sealed class AuthenticationService(ILoggerManager logger, IMapper mapper, UserManager<User> userManager, IConfiguration configuration) : IAuthenticationService
{
    private readonly ILoggerManager _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly UserManager<User> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;
    private User? _user;

    public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration)
    {
        var user = _mapper.Map<User>(userForRegistration);
        var result = await _userManager.CreateAsync(user, userForRegistration.Password);
        if (result.Succeeded)
            await _userManager.AddToRolesAsync(user, userForRegistration.Roles);

        return result;
    }

    public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuth)
    {
        _user = await _userManager.FindByNameAsync(userForAuth.UserName);

        var result = (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuth.Password));
        if (!result)
            _logger.LogWarn($"{nameof(ValidateUser)}: Authentication failed. Wrong user name or password.");

        return result;
    }

    public async Task<TokenDto> CreateToken(bool populateExp)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaimsAsync();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        var refreshToken = GenerateRefreshToken();
        _user.RefreshToken = refreshToken;

        if (populateExp)
            _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        
        await _userManager.UpdateAsync(_user);
        
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new TokenDto(accessToken, refreshToken);
    }

    public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
    {
        var claimsPrincipal = GetClaimsPrincipalFromAccessToken(tokenDto.AccessToken);
        var user = await _userManager.FindByNameAsync(claimsPrincipal.Identity.Name);
        if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            throw new RefreshTokenBadRequest();
        _user = user;

        return await CreateToken(populateExp: false);
    }

    private static SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("WEB_API_SECRET"));
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaimsAsync()
    {
        List<Claim> claims =
            [
                new Claim(ClaimTypes.Name, _user.UserName)
            ];

        var roles = await _userManager.GetRolesAsync(_user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var tokenOptions = new JwtSecurityToken
        (
            issuer: jwtSettings["validIssuer"],
            audience: jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal GetClaimsPrincipalFromAccessToken(string token)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("WEB_API_SECRET"))),
            ValidateLifetime = true,
            ValidIssuer = jwtSettings["validIssuer"],
            ValidAudience = jwtSettings["validAudience"]
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return claimsPrincipal;
    }

}
