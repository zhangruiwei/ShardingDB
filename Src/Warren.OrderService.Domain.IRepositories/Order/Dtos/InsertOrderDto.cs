using System;

namespace Warren.OrderService.Domain.IRepositories.Order.Dtos
{
    public class InsertOrderDto
    {
        public string shipper_number { get; set; }
        public DateTime forecast_date { get; set; }
    }
}
