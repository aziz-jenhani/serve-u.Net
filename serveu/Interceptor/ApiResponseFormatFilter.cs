using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace serveu.Interceptor
{
    public class ApiResponseFormatFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var objectResult = context.Result as ObjectResult;

            if (objectResult != null)
            {
                var statusCode = objectResult.StatusCode ?? StatusCodes.Status200OK;

                var apiResponse = new
                {
                    StatusCode = statusCode,
                    Success = statusCode >= 200 && statusCode < 400,
                    Data = objectResult.Value,
                };

                context.Result = new ObjectResult(apiResponse)
                {
                    StatusCode = statusCode,
                    DeclaredType = typeof(object),
                };
            }

            await next();
        }
    }
}
