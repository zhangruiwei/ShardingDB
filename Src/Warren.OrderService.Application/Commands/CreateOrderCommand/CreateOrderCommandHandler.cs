using System.Threading;
using System.Threading.Tasks;
using Warren.OrderService.Application.Models;
using Warren.OrderService.Domain.IRepositories.Order;

namespace Warren.OrderService.Application.Commands.CreateOrderCommand
{
    public class CreateOrderCommandHandler : MediatR.IRequestHandler<CreateOrderCommand, IApiResult>
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
        }

        public virtual async Task<IApiResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var result = await _orderRepository.Insert(new Domain.IRepositories.Order.Dtos.InsertOrderDto
            {
                shipper_number = request.shipper_number,
                forecast_date = request.forecast_date
            });

            if (result)
            {
                var dateResult = await _orderRepository.InsertByDate(new Domain.IRepositories.Order.Dtos.InsertOrderDto
                {
                    shipper_number = request.shipper_number,
                    forecast_date = request.forecast_date
                });

                return EnumApiStatus.BizOK.ToOkApiResult();
            }
            else
            {
                return EnumApiStatus.BizError.ToOkApiResult();
            }
        }
    }
}
