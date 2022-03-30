namespace Warren.OrderService.Application.Models
{
    public class PagingRequest
    {
        /// <summary>
        ///     当前页
        /// </summary>
        public int page_index { get; set; }

        /// <summary>
        ///     页数
        /// </summary>
        public int page_size { get; set; }
    }

    public class PagingRequest<T> : PagingRequest
    {
        public virtual T data { get; set; }
    }
}
