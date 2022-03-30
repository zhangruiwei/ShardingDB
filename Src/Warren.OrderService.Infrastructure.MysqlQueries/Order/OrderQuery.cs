using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warren.OrderService.Domain.IQueries.Order.Dtos;
using Warren.OrderService.Infrastructure.Hash;
using Warren.OrderService.Infrastructure.Mysql.DbContext;

namespace Warren.OrderService.Infrastructure.MysqlQueries.Order
{
    public class OrderQuery : DbContext, Domain.IQueries.Order.IOrderQuery
    {
        private readonly IConfiguration configuration;

        public OrderQuery(IConfiguration configuration) : base(configuration)
        {
            this.configuration = configuration;
        }


        public async Task<Domain.IQueries.Dtos.Common.PagingResponse<List<QueryOrderInfoDto>>> QueryOrderInfoByPaging(Domain.IQueries.Dtos.Common.PagingRequest<QueryOrderInfoConditionDto> condition)
        {
            var queryOrderInfoDtos = new List<QueryOrderInfoDto>();
            var queryOrderInfoCondition = condition.Data;
            var sql = @"select * from [orderservice_date]{dates}.order_info ";
            var whereSQL = new List<string>() { " forecast_date>=@begin_date and forecast_date<=@end_date " };
            string orderBySQL = "order by forecast_date desc";
            var dates = _shardingHelper.DateTimeIntervalSplit(queryOrderInfoCondition.begin_date, queryOrderInfoCondition.end_date).OrderByDescending(o => o);
            var result = await QueryByPagingAsync<QueryOrderInfoDto>(string.Concat(sql, " WHERE ", string.Join(" AND ", whereSQL), orderBySQL), condition.page_index, condition.page_size,
                new
                {
                    dates,
                    queryOrderInfoCondition.begin_date,
                    queryOrderInfoCondition.end_date
                });

            return new Domain.IQueries.Dtos.Common.PagingResponse<List<QueryOrderInfoDto>>() { Data = result.Data, total = result.total };
        }

        public async Task<List<QueryOrderInfoDto>> QueryOrderInfoByNumbers(List<string> numbers)
        {
            List<QueryOrderInfoDto> queryOrderInfoDtos = new List<QueryOrderInfoDto>();
            var orderIds = await QueryOrderIdByNumber(numbers);
            if (orderIds.Any())
            {
                queryOrderInfoDtos.AddRange(await QueryOrderInfoDtosByIds(orderIds));
            }

            return queryOrderInfoDtos;
        }


        private async Task<List<ulong>> QueryOrderIdByNumber(List<string> number)
        {
            var results = new List<ulong>();
            var index = number.Select(o => MD5Hash.Md5Hash(o)).ToList();
            var sql = @"select order_id from [orderservice_index]{index}.order_index_shipper{index} where shipper_number in @number;";
            var mulits = await QueryMultipleAsync((Dapper.SqlMapper.GridReader mulit) =>
            {
                List<ulong> temp_result = new List<ulong>();
                using (mulit)
                {
                    if (!mulit.IsConsumed)
                    {
                        temp_result.AddRange(mulit.Read<ulong>());
                    }
                }
                return temp_result;
            }, sql, new { number, index });
            foreach (var mulit in mulits)
            {
                results.AddRange(mulit);
            }
            return results.Distinct().ToList();
        }

        private async Task<List<QueryOrderInfoDto>> QueryOrderInfoDtosByIds(List<ulong> orderIds)
        {
            var sql = @"select
order_info.order_id,
order_info.shipper_number,
order_info.forecast_date
    FROM [orderservice_index]{orderIds}.order_info{orderIds} order_info
WHERE
	order_info.order_id in @orderIds";
            return await QueryAsync<QueryOrderInfoDto>(sql, new { orderIds });
        }

    }
}
