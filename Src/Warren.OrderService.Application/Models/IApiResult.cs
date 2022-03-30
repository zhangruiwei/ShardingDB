using Warren.OrderService.Infrastructure.Externsions;

namespace Warren.OrderService.Application.Models
{
    public interface IApiResult
    {
        /// <summary>
        /// 接口业务状态
        /// </summary>
        EnumApiStatus code { get; set; }

        /// <summary>
        /// 消息状态说明
        /// </summary>
        string message { get; set; }
    }

    /// <summary>
    /// API返回消息基类
    /// </summary>
    public class FailApiResult : IApiResult
    {
        /// <summary>
        /// 接口业务状态
        /// </summary>
        public EnumApiStatus code { get; set; }

        /// <summary>
        /// 消息状态说明
        /// </summary>
        public string message { get; set; }
    }

    /// <summary>
    /// API返回消息基类
    /// </summary>
    public class OkApiResult<T> : IApiResult
    {
        public OkApiResult(EnumApiStatus code, string message, T data, long? total = null)
        {

            this.code = code;
            this.message = message;
            this.data = data;
            this.total = total;
        }

        /// <summary>
        /// 接口业务状态
        /// </summary>
        public EnumApiStatus code { get; set; }

        /// <summary>
        /// 消息状态说明
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T data { get; set; }

        public long? total { get; set; }
    }
    public static class ApiResultExtersion
    {
        public static FailApiResult ToOkApiResult(this EnumApiStatus code, string message = "")
        {
            return new FailApiResult() { code = code, message = string.IsNullOrEmpty(message) ? code.GetEnumDescript() : message };
        }

        public static OkApiResult<T> ToOkApiResult<T>(this EnumApiStatus code, string message = "")
        {
            return new OkApiResult<T>(code, string.IsNullOrEmpty(message) ? code.GetEnumDescript() : message, default(T), null);
        }

        public static OkApiResult<T> ToOkApiResult<T>(this EnumApiStatus code, T data, string message = "")
        {
            return new OkApiResult<T>(code, string.IsNullOrEmpty(message) ? code.GetEnumDescript() : message, data, null);
        }

        public static OkApiResult<T> ToOkApiResult<T>(this EnumApiStatus code, T data, long total, string message = "")
        {
            return new OkApiResult<T>(
                code: code,
                message: string.IsNullOrEmpty(message) ? code.GetEnumDescript() : message,
                data: data,
                total: total);
        }
    }
}
