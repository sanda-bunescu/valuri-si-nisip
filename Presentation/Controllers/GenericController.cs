using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Controllers;

public class GenericController : Controller
{
    public AuthInfo AuthInfo { get; set; } = new AuthInfo();
    
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userId = User
            .Claims
            .FirstOrDefault(x => string.Equals(x.Type, "accountId"))?.Value;

        if (userId != null)
        {
            AuthInfo.UserId = Guid.Parse(userId);
        }

        await base.OnActionExecutionAsync(context, next);
    }
}