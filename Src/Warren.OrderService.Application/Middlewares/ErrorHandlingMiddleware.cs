using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using Warren.OrderService.Application.Models;

namespace Warren.OrderService.Application.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private ExceptionHandlerOptions _options;
        public ErrorHandlingMiddleware(RequestDelegate next, IOptions<ExceptionHandlerOptions> options)
        {
            this.next = next;
            this._options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                var result = JsonConvert.SerializeObject(EnumApiStatus.BizError.ToOkApiResult($"{ex.Message}.\n\r {string.Join(";", ex.Errors.Select(a => a.ErrorMessage).ToList())}"));
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json;charset=utf-8";
                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                var result = JsonConvert.SerializeObject(EnumApiStatus.BizError.ToOkApiResult(ex.Message));
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json;charset=utf-8";
                await context.Response.WriteAsync(result);
            }
        }
    }
}
