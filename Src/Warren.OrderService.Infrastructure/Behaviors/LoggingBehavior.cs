using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Warren.OrderService.Infrastructure.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                var response = await next();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
