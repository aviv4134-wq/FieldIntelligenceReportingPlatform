using Microsoft.AspNetCore.Diagnostics;

namespace ReportApi.middleware
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        private ILogger<CustomExceptionHandler> _logger;
        public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
           )
        {
            
            if (exception is InvalidOperationException elasticErorr)
            {
                _logger.LogError("error with the  elastic server {elasticErorr.Message}", elasticErorr.Message);
                httpContext.Response.StatusCode = 500;

                await httpContext.Response.WriteAsJsonAsync(new
                {
                    
                    error = exception.Message
                },cancellationToken);
                
                return true;

            }

            _logger.LogError("error server accred {exception.Message}", exception.Message);
            httpContext.Response.StatusCode = 500;

            await httpContext.Response.WriteAsJsonAsync(new
            {
                error = $"server error : {exception.Message}"
            },cancellationToken);
                        
            return true;
        }
    }
}
