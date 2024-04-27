using Api.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AdminAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
<<<<<<< HEAD
       
      if (false) { // TODO: Check if user is admin
=======
        var tokenUser = context.HttpContext.User;

        if (tokenUser.Claims.FirstOrDefault(c => c.Type == "Role")!.Value != UserRole.Admin.ToString()) {
>>>>>>> 3aad2bfc3ea582d8e7135b47134837c53f728878
            context.Result = new UnauthorizedResult();
            return;
        }

        // Before
        await next();
        // After
    }
}