using Microsoft.AspNetCore.Diagnostics;

namespace RealStateApp.Presentation.WebApp.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
