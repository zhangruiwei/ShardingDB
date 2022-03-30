using System;
using System.Collections.Generic;
using System.Text;

namespace Warren.OrderService.Infrastructure.Mysql.Dtos
{
    public class PagingResponse
    {
        public int total { get; set; }
    }

    public class PagingResponse<T> : PagingResponse
    {
        public virtual T Data { get; set; }
    }
}
