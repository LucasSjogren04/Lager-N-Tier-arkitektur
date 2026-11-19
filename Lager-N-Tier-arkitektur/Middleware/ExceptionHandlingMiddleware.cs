using Microsoft.AspNetCore.Mvc;

namespace Lager_N_Tier_arkitektur.Middleware
{
    // =================================================================
    //  PRESENTATION LAYER  -  CUSTOM MIDDLEWARE
    // -----------------------------------------------------------------
    //  A "station" in the request pipeline. It wraps everything after it
    //  in try/catch, on the way in and the way out:
    //    ArgumentException (business rule broken) -> 400 Bad Request
    //    any other exception                      -> 500 Server Error
    //
    //  This keeps HTTP in the Presentation layer: the service just
    //  throws and never needs to know about status codes.
    // =================================================================
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // pass the request on to the next middleware
            }
            catch (ArgumentException ex)
            {
                await WriteProblem(context, StatusCodes.Status400BadRequest, "Validation failed", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await WriteProblem(context, StatusCodes.Status500InternalServerError, "Server error", "Something went wrong.");
            }
        }

        private static Task WriteProblem(HttpContext context, int status, string title, string detail)
        {
            context.Response.StatusCode = status;
            return context.Response.WriteAsJsonAsync(
                new ProblemDetails { Status = status, Title = title, Detail = detail },
                options: null,
                contentType: "application/problem+json");
        }
    }
}
