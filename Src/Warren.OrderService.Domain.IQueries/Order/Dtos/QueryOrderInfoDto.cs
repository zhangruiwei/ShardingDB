using System;

namespace Warren.OrderService.Domain.IQueries.Order.Dtos
{
    public class QueryOrderInfoDto
    {
        public long order_id { get; set; }
        public string shipper_number { get; set; }
        public DateTime forecast_date { get; set; }
    }
}
