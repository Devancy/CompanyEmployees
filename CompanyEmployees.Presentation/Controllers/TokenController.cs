using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/token")]
[ApiController]
public class TokenController(IServiceManager service) : ControllerBase
{
    private readonly IServiceManager _service = service;

    [HttpPost("refresh")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
    {
        var tokenDtoToReturn = await
        _service.AuthenticationService.RefreshToken(tokenDto);
        return Ok(tokenDtoToReturn);
    }
}

