using Aspire.Core;
using Aspire.FreeSql.Provider;

namespace AspireAdmin.Core.Users
{
    public class User : AuditEntity, IUserEntity
    {
        public string Account { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
