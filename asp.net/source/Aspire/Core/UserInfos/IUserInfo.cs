using Microsoft.AspNetCore.Identity;

namespace Aspire.Core.UserInfos
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public interface IUserInfo : IAuditEntity, ICurrentLoginUser
    {

    }


    public class TestUser : ICurrentLoginUser
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
