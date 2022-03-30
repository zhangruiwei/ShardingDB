using System.Threading.Tasks;
using Warren.OrderService.Domain.IRepositories.Order.Dtos;

namespace Warren.OrderService.Domain.IRepositories.Order
{
    public interface IOrderRepository
    {
        Task<bool> Insert(InsertOrderDto insertOrderDto);

        Task<bool> InsertByDate(InsertOrderDto insertOrderDto);
    }
}
