using System.Text.Json;
using NfcCardManagement.API.DTOs.Common;
using NfcCardManagement.API.Exceptions;

namespace NfcCardManagement.API.Middleware;

/// <summary>
/// Middleware global de gestion des exceptions.
/// Intercepte toutes les exceptions non gérées et retourne une ApiResponse standardisée.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        int statusCode;
        string message;

        switch (exception)
        {
            case NotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                message = exception.Message;
                _logger.LogWarning("Ressource non trouvée : {Message}", exception.Message);
                break;

            case ConflictException:
                statusCode = StatusCodes.Status409Conflict;
                message = exception.Message;
                _logger.LogWarning("Conflit métier : {Message}", exception.Message);
                break;

            case UnprocessableEntityException:
                statusCode = StatusCodes.Status422UnprocessableEntity;
                message = exception.Message;
                _logger.LogWarning("Entité non traitable : {Message}", exception.Message);
                break;

            default:
                statusCode = StatusCodes.Status500InternalServerError;
                message = "Une erreur interne est survenue.";
                _logger.LogError(exception, "Erreur non gérée.");
                break;
        }

        var response = ApiResponse.Fail(message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
    }
}
