using Aspire.FreeSql.Provider;

namespace AspireAdmin.Core.Users
{
    public class UserInfo : AuditEntity
    {
        public string UserName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }
}
