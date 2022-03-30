using System;

namespace Warren.OrderService.Domain.Entities.Order
{
    public class order_info
    {
        public ulong order_id { get; set; }
        public string shipper_number { get; set; }
        public DateTime forecast_date { get; set; }
    }
}
