using Microsoft.AspNetCore.Diagnostics;

namespace ReportApi.middleware
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        public CustomExceptionHandler()
        {
        }
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            
            if (exception is InvalidOperationException elasticErorr)
            {

                httpContext.Response.StatusCode = 500;

                await httpContext.Response.WriteAsJsonAsync(new
                {
                    //error = "the elastic search server is offline"
                    error = exception.Message
                },cancellationToken);
                
                return true;

            }

            httpContext.Response.StatusCode = 500;

            await httpContext.Response.WriteAsJsonAsync(new
            {
                error = $"server error : {exception.Message}"
            },cancellationToken);
                        
            return true;
        }
    }
}
