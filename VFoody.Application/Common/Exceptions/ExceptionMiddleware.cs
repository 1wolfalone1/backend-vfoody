using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using Newtonsoft.Json;
using VFoody.Domain.Exceptions.Base;
using VFoody.Domain.Shared;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace VFoody.Application.Common.Exceptions;

public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;
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
        catch (ApiException exception)
        {
            _logger.LogError(exception, exception.Message);
            await HandleApiExceptionAsync(context, exception);
        }
        catch (ValidationException exception)
        {
            _logger.LogError(exception, exception.Message);
            await HandleValidationExceptionASync(context, exception);
        }
        catch (BadRequestException exception)
        {
            _logger.LogError(exception, exception.Message);
            await HandleBadRequestExceptionASync(context, exception);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await HandleInternalServerExceptionAsync(context, exception);
        }
    }

    private async Task HandleInternalServerExceptionAsync(HttpContext context, Exception exception)
    {
        await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, new ExceptionResponse(exception));
    }

    private async Task HandleValidationExceptionASync(HttpContext context, ValidationException exception)
    {
        await HandleExceptionAsync(context, HttpStatusCode.BadRequest, new ExceptionResponse(exception));
    }
    
    private async Task HandleBadRequestExceptionASync(HttpContext context, BadRequestException exception)
    {
        await HandleExceptionAsync(context, HttpStatusCode.BadRequest, new ExceptionResponse(exception));
    }

    private async Task HandleApiExceptionAsync(HttpContext context, ApiException exception)
    {
        await HandleExceptionAsync(context, HttpStatusCode.BadRequest, new ExceptionResponse(exception));
    }

    private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode code, ExceptionResponse response)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true};  
        await context.Response.WriteAsync(JsonSerializer.Serialize(Result.Failure(new Error(response.ErrorCode.ToString(),
            response.Details != null ? JsonConvert.SerializeObject( response.Details ) : response.Message)), options));
    }
}
