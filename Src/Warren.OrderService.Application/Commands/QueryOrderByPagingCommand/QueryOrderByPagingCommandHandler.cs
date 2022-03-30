using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Warren.OrderService.Application.Models;
using Warren.OrderService.Domain.IQueries.Order;

namespace Warren.OrderService.Application.Commands.QueryOrderByPagingCommand
{
    public class QueryOrderByPagingCommandHandler : IRequestHandler<QueryOrderByPagingCommand, OkApiResult<List<QueryOrderByPagingCommandResponse>>>
    {
        private readonly IOrderQuery _orderQuery;

        public QueryOrderByPagingCommandHandler(IOrderQuery orderQuery)
        {
            this._orderQuery = orderQuery;
        }

        public async Task<OkApiResult<List<QueryOrderByPagingCommandResponse>>> Handle(QueryOrderByPagingCommand request, CancellationToken cancellationToken)
        {
            if (request.data.order_numbers != null && request.data.order_numbers.Any())
            {
                var result = new List<Domain.IQueries.Order.Dtos.QueryOrderInfoDto>();
                var retutn_result = new List<Domain.IQueries.Order.Dtos.QueryOrderInfoDto>();

                //Redis缓存结果集

                if (!result.Any())
                {
                    result = await _orderQuery.QueryOrderInfoByNumbers(request.data.order_numbers);

                    result = result.OrderByDescending(o => o.forecast_date).ToList();
                }
                if (result.Any())
                {
                    retutn_result = result.Skip((request.page_index - 1) * request.page_size).Take(request.page_size).ToList();
                }

                return EnumApiStatus.BizOK.ToOkApiResult(QueryOrderByPagingCommandResponse.MapFrom(retutn_result), result.Count);
            }
            else
            {
                var condition = new Domain.IQueries.Dtos.Common.PagingRequest<Domain.IQueries.Order.Dtos.QueryOrderInfoConditionDto>()
                {
                    Data = new Domain.IQueries.Order.Dtos.QueryOrderInfoConditionDto
                    {
                        begin_date = request.data.begin_date.Value,
                        end_date = request.data.end_date.Value

                    },
                    page_index = request.page_index,
                    page_size = request.page_size
                };

                var queryOrderInfoByPagintDtos = await _orderQuery.QueryOrderInfoByPaging(condition);
                return EnumApiStatus.BizOK.ToOkApiResult(QueryOrderByPagingCommandResponse.MapFrom(queryOrderInfoByPagintDtos.Data), queryOrderInfoByPagintDtos.total);
            }
        }
    }
}
