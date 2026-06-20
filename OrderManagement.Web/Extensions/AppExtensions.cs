using OrderManagement.API.WebApi.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace OrderManagement.Web.Extensions
{
    public static class AppExtensions
    {

        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}