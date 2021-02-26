using Aspire.Core.Authenticate;
using Aspire.FreeSql.Provider;

namespace AspireAdmin.Core.Users
{
    public class User : AuditEntity, IUserEntity
    {
        public string Account { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
    }

    public class UserRole : AuditEntity, IUserRoleEntity
    {
        public string RoleName { get; set; }
    }
}
