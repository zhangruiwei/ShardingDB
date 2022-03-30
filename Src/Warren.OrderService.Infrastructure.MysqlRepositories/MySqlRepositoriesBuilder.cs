using Autofac;
using Warren.OrderService.Infrastructure.MysqlRepositories;
using Warren.OrderService.Infrastructure.MysqlRepositories.Order;

namespace Warren.OrderService.Infrastructure.MysqlRepositories
{
    public class MySqlRepositoriesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OrderRepository>();
            builder.RegisterAssemblyTypes(typeof(Warren.OrderService.Infrastructure.MysqlRepositories.Order.OrderRepository).Assembly)
                 .Where(t => t.Name.EndsWith("Repository"))
                 .AsImplementedInterfaces();
        }
    }
}



namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class DependencyInjectionExtersion
    {
        public static void AddMySqlRepositoriesModule(this ContainerBuilder container)
        {
            container.RegisterModule<MySqlRepositoriesModule>();
        }
    }
}