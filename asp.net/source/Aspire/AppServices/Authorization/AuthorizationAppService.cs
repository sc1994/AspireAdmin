using System;
using System.Threading.Tasks;

using Aspire.Authenticate;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Aspire.Authorization
{
    /// <summary>
    /// 鉴权
    /// </summary>
    abstract public class AuthorizationAppService<TUserEntity> :
        AuthorizationAppService<
            TUserEntity,
            Guid,
            LoginDto,
            CurrentUserDto,
            RegisterDto>
        where TUserEntity : class, IUserEntity, new()
    {

    }

    /// <summary>
    /// 鉴权
    /// </summary>
    abstract public class AuthorizationAppService<
        TUserEntity,
        TLoginDto,
        TCurrentUserDto,
        TRegisterDto> :
        AuthorizationAppService<
        TUserEntity,
        Guid,
        TLoginDto,
        TCurrentUserDto,
        TRegisterDto>
        where TUserEntity : class, IUserEntity, new()
        where TLoginDto : LoginDto
        where TCurrentUserDto : CurrentUserDto
        where TRegisterDto : RegisterDto
    {

    }

    /// <summary>
    /// 鉴权
    /// </summary>
    abstract public class AuthorizationAppService<
        TUserEntity,
        TPrimaryKey,
        TLoginDto,
        TCurrentUserDto,
        TRegisterDto> : Application
        where TUserEntity : class, IUserEntity<TPrimaryKey>, new()
        where TLoginDto : LoginDto
        where TCurrentUserDto : CurrentUserDto
        where TRegisterDto : RegisterDto
    {
        private readonly AspireAppSettings _aspireAppSettings;
        private readonly IAuditRepository<TUserEntity, TPrimaryKey> _userRepository;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// 鉴权
        /// </summary>
        protected AuthorizationAppService()
        {
            _aspireAppSettings = ServiceLocator.ServiceProvider.GetService<IOptions<AspireAppSettings>>().Value;
            _userRepository = ServiceLocator.ServiceProvider.GetService<IAuditRepository<TUserEntity, TPrimaryKey>>();
            _logWriter = ServiceLocator.ServiceProvider.GetService<ILogWriter>();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        virtual async public Task<TokenDto> LoginAsync(TLoginDto input)
        {
            _logWriter.Information("Information", "xx", "cc");
            _logWriter.Warning("Warning", "xx", "cc");
            _logWriter.Error(new Exception(), "Error", "xx", "cc");

            if (!TryAdminLogin(input, out var user)) {
                user = await TryUserLogin(input);
            }

            if (user is null) {
                return Failure<TokenDto>(ResponseCode.UnauthorizedAccountOrPassword, "用户名或者密码错误");
            }

            return new JwtManage(_aspireAppSettings.Jwt).GenerateJwtToken(user);
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        virtual async public Task<TCurrentUserDto> GetCurrentUserAsync([FromServices] ICurrentUser currentUser)
        {
            var user = await _userRepository
                .GetBatchAsync(x => x.Account == currentUser.Account, 1)
                .FirstOrDefaultAsync();
            return MapTo<TCurrentUserDto>(user);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        virtual public string Logout()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AuthorizeFilter(Roles.Admin)]
        virtual async public Task<bool> RegisterAsync(TRegisterDto input)
        {
            return await _userRepository.InsertAsync(new TUserEntity {
                Account = input.Account,
                Password = input.Password,
                Roles = input.Roles,
                Name = input.Name
            });
        }

        /// <summary>
        /// 尝试用户登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task<TUserEntity> TryUserLogin(TLoginDto input)
        {
            return await _userRepository
                .GetBatchAsync(x => x.Account == input.Account && x.Password == input.Password, 1)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 尝试管理员登陆
        /// </summary>
        /// <param name="input"></param>
        /// <param name="userEntity"></param>
        /// <returns></returns>
        protected virtual bool TryAdminLogin(TLoginDto input, out TUserEntity userEntity)
        {
            if (input.Password == _aspireAppSettings.Administrator.Password
                && input.Account == _aspireAppSettings.Administrator.Account) {
                userEntity = new TUserEntity {
                    Account = _aspireAppSettings.Administrator.Account,
                    Name = _aspireAppSettings.Administrator.Name,
                    Roles = Roles.Admin
                };
                return true;
            }

            userEntity = default;
            return false;
        }
    }
}
