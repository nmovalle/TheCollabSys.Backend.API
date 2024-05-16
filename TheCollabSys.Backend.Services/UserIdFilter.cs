using Microsoft.AspNetCore.Mvc.Filters;
using TheCollabSys.Backend.Entity;

namespace TheCollabSys.Backend.Services;

public class UserIdFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Obtener el User-Id del encabezado de la solicitud
        var userId = context.HttpContext.Request.Headers["User-Id"];

        // Iterar sobre los argumentos de la acción para encontrar objetos que implementen IUserOwned
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is IUserOwned userOwned)
            {
                // Asignar el User-Id al objeto IUserOwned
                userOwned.UserId = userId;
            }
        }

        // Continuar con la ejecución de la acción
        var resultContext = await next();
    }
}
