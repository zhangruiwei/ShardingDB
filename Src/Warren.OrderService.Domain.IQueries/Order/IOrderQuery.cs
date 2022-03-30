using System.Collections.Generic;
using System.Threading.Tasks;
using Warren.OrderService.Domain.IQueries.Order.Dtos;

namespace Warren.OrderService.Domain.IQueries.Order
{
    public interface IOrderQuery
    {
        Task<Domain.IQueries.Dtos.Common.PagingResponse<List<QueryOrderInfoDto>>> QueryOrderInfoByPaging(Domain.IQueries.Dtos.Common.PagingRequest<QueryOrderInfoConditionDto> condition);

        Task<List<QueryOrderInfoDto>> QueryOrderInfoByNumbers(List<string> numbers);

    }
}
