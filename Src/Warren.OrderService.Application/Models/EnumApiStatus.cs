using System.ComponentModel;

namespace Warren.OrderService.Application.Models
{
    public enum EnumApiStatus
    {
        #region 默认业务状态 0~1
        [Description("操作成功")]
        BizOK = 0,

        /// <summary>
        /// 操作失败
        /// </summary>
        [Description("操作失败")]
        BizError = 1,
        #endregion

        #region 系统接口状态 2~99

        /// <summary>
        /// 接口参数数据验证失败
        /// </summary>
        [Description("接口数据验证失败")]
        ApiParamModelValidateError = 4,

        /// <summary>
        /// 用户未登录
        /// </summary>
        [Description("用户未登录")]
        ApiUserNotLogin = 5,
        /// <summary>
        /// 用户无权限访问
        /// </summary>
        [Description("用户无权限访问")]
        ApiUserUnauthorized = 6,

        #endregion   
    }
}
