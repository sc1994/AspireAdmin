using Aspire.Authorization;

using AspireAdmin.Core.Users;

namespace AspireAdmin.Application.Aspires
{
    /// <summary>
    /// 鉴权
    /// </summary>
    public class AuthorizationAppService : AuthorizationAppService<User, CustomLoginDto, CustomCurrentUserDto, CustomRegisterDto>
    {

    }
}
