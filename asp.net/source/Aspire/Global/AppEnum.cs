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
        /// 未授权的账号或者密码
        /// </summary>
        UnauthorizedAccountOrPassword = 400001,

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
