using System;

namespace Warren.OrderService.Domain.IQueries.Order.Dtos
{
    public class QueryOrderInfoConditionDto
    {
        public DateTime begin_date { get; set; }
        public DateTime end_date { get; set; }
    }
}
