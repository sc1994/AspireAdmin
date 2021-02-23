namespace Aspire.Core.UserInfos
{
    /// <summary>
    /// 用户信息管理
    /// </summary>
    public class UserInfoManage
    {
        public ICurrentLoginUser GetCurrentLoginUserByToken(string Token)
        {
            return new TestUser();
        }
    }
}
