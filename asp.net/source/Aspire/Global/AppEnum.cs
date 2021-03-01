namespace Aspire
{
    /// <summary>
    /// 响应编码
    /// </summary>
    public enum ResponseCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Ok = 200,

        /// <summary>
        /// 未授权
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// 未授权角色
        /// </summary>
        UnauthorizedRoles = 403,

        /// <summary>
        /// 未授权的账号或者密码
        /// </summary>
        UnauthorizedAccountOrPassword = 401001,

        /// <summary>
        /// 内部服务异常
        /// </summary>
        InternalServerError = 500,

        /// <summary>
        /// 内部服务数据库异常
        /// </summary>
        InternalServerDatabaseError = 500001
    }
}
