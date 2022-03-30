namespace Warren.OrderService.Infrastructure.Mysql.Dtos
{
    public class BatchExecuteScalar
    {
        public string sql { get; set; }

        public object param { get; set; }
    }
}
