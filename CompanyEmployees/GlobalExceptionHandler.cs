using Contracts;
using Entities.ErrorModel;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;

namespace CompanyEmployees;

public class GlobalExceptionHandler(ILoggerManager logger) : IExceptionHandler
{
    private readonly ILoggerManager _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";

        var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature != null)
        {
            httpContext.Response.StatusCode = contextFeature.Error switch
            {
                IErrorStatusCode ex => ex.GetErrorStatusCode(),
                SecurityTokenValidationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
            _logger.LogError($"Something went wrong: {exception.Message}");

            await httpContext.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = contextFeature.Error.Message,
            }.ToString());
        }

        return true;
    }
}
