﻿using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.Common;
using SWD.TicketBooking.Service.Exceptions;


namespace SWD.TicketBooking.API.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private readonly IDictionary<Type, Action<HttpContext, Exception>> _exceptionHandlers = new Dictionary<Type, Action<HttpContext, Exception>>
    {
        // Note: Handle every exception you throw here
        
        // a NotFoundException is thrown when the resource requested by the client
        // cannot be found on the resource server
        { typeof(NotFoundException), HandleNotFoundException },

        { typeof(BadRequestException), HandleBadRequestException },

        { typeof(InternalServerErrorException), HandleInternalServeErrorException },
    };

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        var type = ex.GetType();
        if (_exceptionHandlers.TryGetValue(type, out var handler))
        {
            handler.Invoke(context, ex);
            return Task.CompletedTask;
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        Console.WriteLine(ex.ToString());
        
        return WriteExceptionMessageAsync(context, ex);
    }


    private static async void HandleNotFoundException(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await WriteExceptionMessageAsync(context, ex);
    }

    private static async void HandleBadRequestException(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await WriteExceptionMessageAsync(context, ex);
    }

    private static async void HandleInternalServeErrorException(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await WriteExceptionMessageAsync(context, ex);
    }


    private static async Task WriteExceptionMessageAsync(HttpContext context, Exception ex)
    {
        await context.Response.Body.WriteAsync(SerializeToUtf8BytesWeb(ApiResult<string>.Fail(ex)));
    }

    private static byte[] SerializeToUtf8BytesWeb<T>(T value)
    {
        return JsonSerializer.SerializeToUtf8Bytes(value, new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }
}