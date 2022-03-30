using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Threading.Tasks;

namespace Warren.OrderService.Infrastructure.Interceptors
{
    public class PollyInterceptor : IInterceptor
    {
        private readonly ILogger<PollyInterceptor> logger;

        public PollyInterceptor(ILogger<PollyInterceptor> logger)
        {
            this.logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {

            ISyncPolicy syncPolicy = Policy.Handle<Exception>().WaitAndRetry(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            syncPolicy.Execute(() =>
            {
                invocation.Proceed();
                if (invocation.ReturnValue is Task)
                {
                    logger.LogDebug(invocation.Method.Name, invocation.Arguments);
                    var result = invocation.ReturnValue as Task;
                    if (result.Exception != null)
                    {
                        logger.LogError(result.Exception.Message, result.Exception);
                        throw result.Exception;
                    }
                }
            });

        }
    }
}
