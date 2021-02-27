using System;
using System.Threading.Tasks;

using Aspire.Core.Authenticate;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Aspire.AppServices.Authenticate
{
    /// <summary>
    /// 鉴权
    /// </summary>
    public abstract class AuthenticateAppService<TUserEntity, TUserRoleEntity> : AuthenticateAppService<TUserEntity, TUserRoleEntity, Guid>
        where TUserEntity : class, IUserEntity<Guid>, new()
        where TUserRoleEntity : class, IUserRoleEntity<Guid>, new()
    {

    }

    /// <summary>
    /// 鉴权
    /// </summary>
    public abstract class AuthenticateAppService<TUserEntity, TUserRoleEntity, TPrimaryKey> : AppService
        where TUserEntity : class, IUserEntity<TPrimaryKey>, new()
        where TUserRoleEntity : class, IUserRoleEntity<TPrimaryKey>, new()
    {
        private readonly AspireAppSettings _aspireAppSettings;
        private readonly UserManager<TUserEntity> _userManager;
        private readonly RoleManager<TUserRoleEntity> _roleManager;

        /// <summary>
        /// 鉴权
        /// </summary>
        protected AuthenticateAppService()
        {
            _aspireAppSettings = ServiceLocator.ServiceProvider.GetService<IOptions<AspireAppSettings>>().Value;
            _userManager = ServiceLocator.ServiceProvider.GetService<UserManager<TUserEntity>>();
            _roleManager = ServiceLocator.ServiceProvider.GetService<RoleManager<TUserRoleEntity>>();
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

            return new JwtManage(_aspireAppSettings.Jwt).GenerateJwtToken(user);
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

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<bool> RegisterAsync(RegisterDto input)
        {
            var result = await _userManager.CreateAsync(new TUserEntity {
                Account = input.Account,
                Password = input.Password,
                RoleName = input.UserRole
            });
            return result.Succeeded;
        }


        private async Task<TUserEntity> GetUserByIdAndPwdAsync(LoginDto input)
        {
            var user = await _userManager.FindByNameAsync(input.Account);
            if (user != null && user.Password == input.Password.EncodingToBase64()) { // TODO 密码加密
                return user;
            }
            throw new Exception("用户名或者密码错误");
        }
    }
}
