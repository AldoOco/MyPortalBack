using System.Net;
using System.Text.Json;
using MyPortalBack.Application.Common.Contracts.Responses;
using MyPortalBack.Application.Common.Exceptions;

namespace MyPortalBack.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        HttpStatusCode statusCode;
        ErrorResponse response;

        switch (exception)
        {
            case ValidationException validationException:

                statusCode = HttpStatusCode.BadRequest;

                response = new ErrorResponse(
                    validationException.Message,
                    validationException.Errors);

                break;

            case NotFoundException notFoundException:

                statusCode = HttpStatusCode.NotFound;

                response = new ErrorResponse(
                    notFoundException.Message);

                break;

            case ConflictException conflictException:

                statusCode = HttpStatusCode.Conflict;

                response = new ErrorResponse(
                    conflictException.Message);

                break;

            case UnauthorizedException unauthorizedException:

                statusCode = HttpStatusCode.Unauthorized;

                response = new ErrorResponse(
                    unauthorizedException.Message);

                break;

            case ForbiddenException forbiddenException:

                statusCode = HttpStatusCode.Forbidden;

                response = new ErrorResponse(
                    forbiddenException.Message);

                break;

            default:

                statusCode = HttpStatusCode.InternalServerError;

                response = new ErrorResponse(
                    "Ha ocurrido un error interno del servidor.");

                break;
        }

        context.Response.StatusCode = (int)statusCode;

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}