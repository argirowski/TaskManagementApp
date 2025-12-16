using Application.DTOs;
using Application.Exceptions;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace API.Middleware
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors.Select(e => new ValidationErrorDTO
                {
                    PropertyName = e.PropertyName,
                    ErrorMessage = e.ErrorMessage
                }).ToList();

                var response = new ValidationErrorResponseDTO
                {
                    Errors = errors
                };

                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentType = "application/json";
                var error = new { error = ex.Message };
                var jsonResponse = JsonSerializer.Serialize(error);
                await context.Response.WriteAsync(jsonResponse);
            }
            catch (UnauthorizedException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";
                var error = new { error = ex.Message };
                var jsonResponse = JsonSerializer.Serialize(error);
                await context.Response.WriteAsync(jsonResponse);
            }
            catch (ForbiddenException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.Response.ContentType = "application/json";
                var error = new { error = ex.Message };
                var jsonResponse = JsonSerializer.Serialize(error);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
