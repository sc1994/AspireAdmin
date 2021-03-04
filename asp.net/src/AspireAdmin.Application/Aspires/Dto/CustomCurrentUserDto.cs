using Aspire.Authorization;
using Aspire.Mapper;

using AspireAdmin.Core.Users;

namespace AspireAdmin.Application.Aspires
{
    /// <summary>
    /// 自定义当前用户
    /// </summary>
    [MapperProfile(typeof(User))]
    public class CustomCurrentUserDto : CurrentUserDto
    {

    }
}