using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Warren.OrderService.Domain.Entities.Order;
using Warren.OrderService.Domain.IRepositories.Order.Dtos;
using Warren.OrderService.Infrastructure.Hash;
using Warren.OrderService.Infrastructure.Mysql.DbContext;
using Warren.OrderService.Infrastructure.Mysql.Dtos;

namespace Warren.OrderService.Infrastructure.MysqlRepositories.Order
{
    public class OrderRepository : DbContext, Domain.IRepositories.Order.IOrderRepository
    {
        private readonly IConfiguration configuration;

        public OrderRepository(
           IConfiguration configuration) : base(configuration)
        {
            this.configuration = configuration;
        }

        public async Task<bool> Insert(InsertOrderDto insertOrderDto)
        {
            var batch = new List<BatchExecuteScalar>();
            var order_id = MD5Hash.Md5Hash(insertOrderDto.shipper_number);

            batch.Add(new BatchExecuteScalar
            {
                sql = @"INSERT IGNORE INTO [orderservice_index]{order_id}.`order_info{order_id}` 
(order_id, shipper_number, forecast_date) VALUES 
(@order_id, @shipper_number, @forecast_date);",
                param = new order_info
                {
                    order_id = order_id,
                    shipper_number = insertOrderDto.shipper_number,
                    forecast_date = insertOrderDto.forecast_date
                }
            });

            batch.Add(new BatchExecuteScalar()
            {
                sql = @"INSERT IGNORE INTO [orderservice_index]{order_id}.`order_index_shipper{order_id}`
(`order_id`, `shipper_number`) VALUES
(@order_id, @shipper_number);",
                param = new order_index_shipper
                {
                    order_id = order_id,
                    shipper_number = insertOrderDto.shipper_number
                }
            });

            return await BatchExecuteScalarAsync(batch);
        }

        public async Task<bool> InsertByDate(InsertOrderDto insertOrderDto)
        {
            var batch = new List<BatchExecuteScalar>();
            var order_id = MD5Hash.Md5Hash(insertOrderDto.shipper_number);

            batch.Add(new BatchExecuteScalar
            {
                sql = @"INSERT IGNORE INTO [orderservice_date]{forecast_date}.`order_info` 
(order_id, shipper_number, forecast_date) VALUES 
(@order_id, @shipper_number, @forecast_date);",
                param = new order_info
                {
                    order_id = order_id,
                    shipper_number = insertOrderDto.shipper_number,
                    forecast_date = insertOrderDto.forecast_date
                }
            });

            return await BatchExecuteScalarAsync(batch);
        }
    }
}
