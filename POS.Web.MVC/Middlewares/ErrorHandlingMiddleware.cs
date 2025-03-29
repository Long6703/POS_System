using POS.Web.MVC.Exceptions;
using System.Net;
using System.Text.Json;

namespace POS.Web.MVC.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException ex)
            {
                context.Response.ContentType = "application/json";

                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    context.Response.Redirect("/Auth/Login");
                    return;
                }

                if (ex.StatusCode == HttpStatusCode.Forbidden)
                {
                    context.Response.Redirect("/Error/Forbidden");
                    return;
                }

                context.Response.StatusCode = (int)ex.StatusCode;
                var errorResponse = new { message = ex.Message, statusCode = ex.StatusCode };
                var jsonResponse = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex}");

                context.Response.Redirect("/Error/SomethingWentWrong");
            }
        }
    }
}
