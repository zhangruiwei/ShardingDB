using Autofac;
using Autofac.Extras.DynamicProxy;
using Warren.OrderService.Infrastructure.MysqlQueries;

namespace Warren.OrderService.Infrastructure.MysqlQueries
{
    public class MySqlQueriesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Warren.OrderService.Infrastructure.Interceptors.PollyInterceptor>();
            builder.RegisterAssemblyTypes(typeof(Warren.OrderService.Infrastructure.MysqlQueries.Order.OrderQuery).Assembly)
              .Where(t => t.Name.EndsWith("Query"))
              .AsImplementedInterfaces().EnableInterfaceInterceptors().InterceptedBy(typeof(Interceptors.PollyInterceptor));
        }
    }
}

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class DependencyInjectionExtersion
    {
        public static void AddMySqlQueriesModule(this ContainerBuilder container)
        {
            container.RegisterModule<MySqlQueriesModule>();

        }


    }
}