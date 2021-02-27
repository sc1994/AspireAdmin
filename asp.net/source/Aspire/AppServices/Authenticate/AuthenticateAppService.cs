using System;
using System.Threading.Tasks;

using Aspire.Core.Authenticate;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Aspire.Authenticate
{
    /// <summary>
    /// 鉴权
    /// </summary>
    public abstract class AuthenticateAppService<TUserEntity> : AuthenticateAppService<TUserEntity, Guid>
        where TUserEntity : class, IUserEntity<Guid>, new()
    {

    }

    /// <summary>
    /// 鉴权
    /// </summary>
    public abstract class AuthenticateAppService<TUserEntity, TPrimaryKey> : AppService
        where TUserEntity : class, IUserEntity<TPrimaryKey>, new()
    {
        private readonly AspireAppSettings _aspireAppSettings;
        private readonly IAuditRepository<TUserEntity, TPrimaryKey> _userRepository;

        /// <summary>
        /// 鉴权
        /// </summary>
        protected AuthenticateAppService()
        {
            _aspireAppSettings = ServiceLocator.ServiceProvider.GetService<IOptions<AspireAppSettings>>().Value;
            _userRepository = ServiceLocator.ServiceProvider.GetService<IAuditRepository<TUserEntity, TPrimaryKey>>();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<string> LoginAsync(LoginDto input)
        {
            TUserEntity user;

            if (input.Password == _aspireAppSettings.Administrator.Password
                && input.Account == _aspireAppSettings.Administrator.Account) {
                user = new TUserEntity {
                    Account = _aspireAppSettings.Administrator.Account,
                    Name = _aspireAppSettings.Administrator.Name
                };
            }
            else {
                user = await GetUserByIdAndPwdAsync(input);
            }

            if (user is null) {
                throw new Exception("用户名或者密码错误");
            }

            return new JwtManage(_aspireAppSettings.Jwt).GenerateJwtToken(user);
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<bool> RegisterAsync(RegisterDto input)
        {
            return await _userRepository.InsertAsync(new TUserEntity {
                Account = input.Account,
                Password = input.Password,
                RoleName = input.UserRole,
                Name = input.Name
            });
        }


        private async Task<TUserEntity> GetUserByIdAndPwdAsync(LoginDto input)
        {
            return await _userRepository.GetBatchAsync(
                x => x.Account == input.Account
                && x.Password == input.Password, 1)
                .FirstOrDefaultAsync();
        }
    }
}
