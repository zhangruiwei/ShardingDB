namespace Warren.OrderService.Domain.IQueries.Dtos.Common
{
    public class PagingResponse
    {
        public long total { get; set; }
    }

    public class PagingResponse<T> : PagingResponse
    {
        public virtual T Data { get; set; }
    }
}
