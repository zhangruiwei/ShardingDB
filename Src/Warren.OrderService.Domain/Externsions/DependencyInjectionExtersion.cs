

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtersion
    {
        public static IServiceCollection AddCommon(this IServiceCollection services)
        {
            services.AddTransient<Warren.OrderService.Domain.Interface.ICommonMethods, Warren.OrderService.Domain.Domain.CommonMethods>();
            return services;

        }

    }
}
