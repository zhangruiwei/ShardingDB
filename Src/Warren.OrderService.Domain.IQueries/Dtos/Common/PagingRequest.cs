namespace Warren.OrderService.Domain.IQueries.Dtos.Common
{
    public class PagingRequest
    {
        public int page_index { get; set; }

        public int page_size { get; set; }
    }

    public class PagingRequest<T> : PagingRequest
    {
        public virtual T Data { get; set; }
    }
}
