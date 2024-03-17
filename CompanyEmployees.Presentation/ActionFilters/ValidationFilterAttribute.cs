using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace CompanyEmployees.Presentation.ActionFilters;

[DebuggerStepThrough]
public class ValidationFilterAttribute : IActionFilter
{
    public ValidationFilterAttribute()
    {

    }
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var action = context.RouteData.Values["action"];
        var controller = context.RouteData.Values["controller"];
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var param = context.ActionArguments.SingleOrDefault(x => x.Value.ToString().Contains("Dto")).Value;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        if (param is null)
        {
            context.Result = new BadRequestObjectResult($"Object is null. Controller: {controller}, action: {action}");
            return;
        }
        if (!context.ModelState.IsValid)
            context.Result = new UnprocessableEntityObjectResult(context.ModelState);
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {

    }
}

