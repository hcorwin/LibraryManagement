using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Middleware
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ArgumentException ae)
            {
                var problem = CreateProblemDetails(HttpStatusCode.BadRequest, "Error", "Error With Argument", ae.Message);
                await HandleContextResponse(context, problem);
            }
            catch (KeyNotFoundException ke)
            {
                var problem = CreateProblemDetails(HttpStatusCode.BadRequest, "Error", "Error With Argument", ke.Message);
                await HandleContextResponse(context, problem);
            }
            catch (Exception)
            {
                var problem = CreateProblemDetails(HttpStatusCode.InternalServerError, "Internal Server Error", "Internal Server Error", "Unexpected Error");
                await HandleContextResponse(context, problem);
            }
        }

        private static ProblemDetails CreateProblemDetails(HttpStatusCode status, string type, string title, string detail) =>
            new()
            {
                Status = (int)status,
                Type = type,
                Title = title,
                Detail = detail
            };

        private static async Task HandleContextResponse(HttpContext context, ProblemDetails problem)
        {
            context.Response.StatusCode = (int)problem.Status!;
            var json = JsonSerializer.Serialize(problem);
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsync(json);
        }
    }
}
