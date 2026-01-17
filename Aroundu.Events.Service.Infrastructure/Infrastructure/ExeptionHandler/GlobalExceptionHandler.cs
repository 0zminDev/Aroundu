using Microsoft.AspNetCore.Http;

namespace Aroundu.Events.Service.Infrastructure.Infrastructure.ExeptionHandler
{
    public class GlobalExceptionHandler : Microsoft.AspNetCore.Diagnostics.IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is FluentValidation.ValidationException validationException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new
                {
                    Errors = validationException.Errors.Select(x => x.ErrorMessage)
                }, cancellationToken);
                return true;
            }
            return false;
        }
    }
}