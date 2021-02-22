using Aspire;
using Aspire.Dto;

using AspireAdmin.Core.Users;

using FreeSql;

namespace AspireAdmin.Application.UserManage
{
    /// <summary>
    /// 登录 应用服务
    /// </summary>
    public class UserInfoAppService : CrudAppService<UserInfo>
    {
        /// <inheritdoc />
        protected override ISelect<UserInfo> FilterPage(PageInputDto dto)
        {
            throw new System.NotImplementedException();
        }
    }
}
