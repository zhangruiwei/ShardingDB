using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection;
using Warren.OrderService.Infrastructure.Externsions;

namespace Warren.OrderService.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<Common.Configuration.DatabaseConfiguration>(Configuration.GetSection("mysql"));

            services.AddMediatR(typeof(Application.Commands.CreateOrderCommand.CreateOrderCommand).GetTypeInfo().Assembly);

            services.AddCommon();

            services.AddControllers().AddNewtonsoftJson();

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterGeneric(typeof(Infrastructure.Behaviors.LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(Infrastructure.Behaviors.ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterAssemblyTypes(typeof(Application.Commands.CreateOrderCommand.CreateOrderCommandValidator).GetTypeInfo().Assembly)
                     .Where(t => t.IsClosedTypeOf(typeof(FluentValidation.IValidator<>)))
                     .AsImplementedInterfaces();
            builder.RegisterModules(a =>
            {
                a.AddMySqlQueriesModule();
                a.AddMySqlRepositoriesModule();
            });
            return new AutofacServiceProvider(builder.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddlewares();

            app.UseHttpsRedirection();

            app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(_ => true));

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
