using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Store.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next,
                                   ILogger<ExceptionMiddleware> logger,
                                   IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Pasa la solicitud al siguiente middleware en el pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Loguea el error
                _logger.LogError(ex, ex.Message);

                // Configura la respuesta HTTP
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                // Crea un objeto ProblemDetails para la respuesta de error
                var response = new ProblemDetails
                {
                    Status = 500,
                    Detail = _env.IsDevelopment() ? ex.StackTrace?.ToString() : null,
                    Title = ex.Message,
                };

                // Configura las opciones de serialización JSON
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                // Serializa el objeto ProblemDetails a JSON
                var json = JsonSerializer.Serialize(response, options);

                // Escribe la respuesta JSON en el cuerpo de la respuesta HTTP
                await context.Response.WriteAsync(json);
            }
        }
    }

}
