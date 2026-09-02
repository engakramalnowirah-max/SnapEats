using System.Net;
using System.Text.Json;
using SnapEats.Application.Common.Models;
using SnapEats.Application.Exceptions;
using SnapEats.Domain.Exceptions;

namespace SnapEats.API.Middleware;

public sealed class ExceptionMiddleware
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
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = exception switch
        {
            ValidationException validationEx => new ErrorResponse(
                "Validation Error",
                (int)HttpStatusCode.BadRequest,
                "One or more validation errors occurred.",
                validationEx.Errors),

            UnauthorizedDomainAccessException => new ErrorResponse(
                "Unauthorized",
                (int)HttpStatusCode.Unauthorized,
                exception.Message),

            DomainException domainEx => new ErrorResponse(
                "Domain Error",
                (int)HttpStatusCode.BadRequest,
                domainEx.Message),

            KeyNotFoundException => new ErrorResponse(
                "Not Found",
                (int)HttpStatusCode.NotFound,
                exception.Message),

            _ => new ErrorResponse(
                "Internal Server Error",
                (int)HttpStatusCode.InternalServerError,
                "An error occurred while processing your request.")
        };

        response.StatusCode = errorResponse.StatusCode;

        _logger.LogError(exception, "HTTP {Method} {Path} failed: {Message}",
            context.Request.Method, context.Request.Path, exception.Message);

        var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(json);
    }
}

