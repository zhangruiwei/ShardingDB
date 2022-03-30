using Autofac;
using System;

namespace Warren.OrderService.Infrastructure.Externsions
{
    public static partial class DependencyInjectionExtersion
    {
        public static void RegisterModules(this ContainerBuilder builder, Action<ContainerBuilder> func)
        {
            func(builder);
        }
    }
}
