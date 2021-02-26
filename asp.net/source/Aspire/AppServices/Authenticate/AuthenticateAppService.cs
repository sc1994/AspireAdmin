using System;
using System.Threading.Tasks;

using Aspire.Core.Authenticate;

using Microsoft.AspNetCore.Authorization;

namespace Aspire.AppServices.Authenticate
{
    /// <summary>
    /// 鉴权
    /// </summary>
    public abstract class AuthenticateAppService<TUserEntity> : AuthenticateAppService<TUserEntity, Guid>
        where TUserEntity : IUserEntity, new()
    {

    }

    /// <summary>
    /// 鉴权
    /// </summary>
    public abstract class AuthenticateAppService<TUserEntity, TPrimaryKey> : AppService
        where TUserEntity : IUserEntity<TPrimaryKey>, new()
    {
        private readonly AspireConfigureOptions _aspireConfigureOptions;

        /// <summary>
        /// 鉴权
        /// </summary>
        protected AuthenticateAppService()
        {
            _aspireConfigureOptions = ServiceLocator.ServiceProvider.GetService<AspireConfigureOptions>();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<string> LoginAsync(LoginInputDto input)
        {
            TUserEntity user;

            if (input.Password == _aspireConfigureOptions.Administrator.Password
                && input.Account == _aspireConfigureOptions.Administrator.Account) {
                user = new TUserEntity {
                    Account = _aspireConfigureOptions.Administrator.Account,
                    Name = _aspireConfigureOptions.Administrator.Name
                };
            }
            else {
                user = await GetUserByIdAndPwdAsync(input);
            }

            if (user is not null) {
                return new JwtManage(_aspireConfigureOptions.Jwt).GenerateJwtToken(user);
            }

            throw new Exception("登录失败");
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public CurrentUserDto GetCurrentUser()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public string Logout()
        {
            throw new NotImplementedException();
        }

        private static async Task<TUserEntity> GetUserByIdAndPwdAsync(LoginInputDto input)
        {
            return await ServiceLocator.ServiceProvider.GetService<IAuditRepository<TUserEntity, TPrimaryKey>>()
                .GetBatchAsync(x => x.Account == input.Account && x.Password == input.Password)
                .FirstOrDefaultAsync();
        }
    }
}
