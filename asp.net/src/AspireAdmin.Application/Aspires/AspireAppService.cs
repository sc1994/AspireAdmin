using Aspire.Authorization;
using Aspire.LogQuery;
using Aspire.Serilog.ElasticSearch.Provider;

using AspireAdmin.Core.Users;

namespace AspireAdmin.Application.Aspires
{
    /// <summary>
    /// 鉴权
    /// </summary>
    public class AuthorizationAppService : AuthorizationAppService<User, CustomLoginDto, CustomCurrentUserDto, CustomRegisterDto>
    {

    }

    /// <summary>
    /// 日志查询
    /// </summary>
    public class LogQueryAppService : LogQueryAppService<LogModel>
    {

    }
}
