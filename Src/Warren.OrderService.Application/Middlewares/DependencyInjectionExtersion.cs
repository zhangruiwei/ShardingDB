﻿using Microsoft.AspNetCore.Builder;
using Warren.OrderService.Application.Middlewares;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class DependencyInjectionExtersion
    {
        public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
