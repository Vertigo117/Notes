using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Notes.Api.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Notes.Api.Middleware
{
    public class ErrorHandlingMiddlware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddlware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next?.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex, httpContext);
            }
        }

        private static async Task HandleExceptionAsync(Exception ex, HttpContext httpContext)
        {
            var error = new Error
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace
            };
            var statusCode = (int)HttpStatusCode.InternalServerError;
            await WriteErrorToContext(error, httpContext, statusCode);
        }

        private static async Task WriteErrorToContext(Error error, HttpContext httpContext, int statusCode)
        {
            string json = JsonConvert.SerializeObject(error, JsonSerializationSettings.CamelCase());
            HttpResponse response = httpContext.Response;
            response.StatusCode = statusCode;
            response.ContentType = "application/json";
            await response.WriteAsync(json);
        }
    }
}
