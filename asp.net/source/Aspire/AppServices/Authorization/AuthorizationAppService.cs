// <copyright file="AuthorizationAppService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Authorization
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Aspire.AuditRepository;
    using Aspire.Authenticate;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Authorization.
    /// </summary>
    /// <typeparam name="TUserEntity">User Entity.</typeparam>
    public abstract class AuthorizationAppService<TUserEntity> :
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
    /// Authorization.
    /// </summary>
    /// <typeparam name="TUserEntity">User Entity.</typeparam>
    /// <typeparam name="TLoginDto">Login Dto.</typeparam>
    /// <typeparam name="TCurrentUserDto">CurrentUser Dto.</typeparam>
    /// <typeparam name="TRegisterDto">Register Dto.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<挂起>")]
    public abstract class AuthorizationAppService<
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
    /// Authorization.
    /// </summary>
    /// <typeparam name="TUserEntity">User Entity.</typeparam>
    /// <typeparam name="TPrimaryKey">Primary Key.</typeparam>
    /// <typeparam name="TLoginDto">Login Dto.</typeparam>
    /// <typeparam name="TCurrentUserDto">CurrentUser Dto.</typeparam>
    /// <typeparam name="TRegisterDto">Register Dto.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<挂起>")]
    public abstract class AuthorizationAppService<
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
        private readonly AspireAppSettings aspireAppSettings;
        private readonly IAuditRepository<TUserEntity, TPrimaryKey> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationAppService{        TUserEntity,         TPrimaryKey,         TLoginDto,         TCurrentUserDto,         TRegisterDto}"/> class.
        /// </summary>
        protected AuthorizationAppService()
        {
            this.aspireAppSettings = ServiceLocator.ServiceProvider.GetService<IOptions<AspireAppSettings>>().Value;
            this.userRepository = ServiceLocator.ServiceProvider.GetService<IAuditRepository<TUserEntity, TPrimaryKey>>();
        }

        /// <summary>
        /// 登录.
        /// </summary>
        /// <param name="input">Login Dto.</param>
        /// <returns>token 相关.</returns>
        [AllowAnonymous]
        public virtual async Task<TokenDto> LoginAsync(TLoginDto input)
        {
            if (!this.TryAdminLogin(input, out var user))
            {
                user = await this.TryUserLogin(input);
            }

            if (user is null)
            {
                return Failure<TokenDto>(ResponseCode.UnauthorizedAccountOrPassword, "用户名或者密码错误");
            }

            return new JwtManage(this.aspireAppSettings.Jwt).GenerateJwtToken(user);
        }

        /// <summary>
        /// 获取当前用户.
        /// </summary>
        /// <param name="currentUser">Current User.</param>
        /// <returns>当前用户.</returns>
        public virtual async Task<TCurrentUserDto> GetCurrentUserAsync([FromServices] ICurrentUser currentUser)
        {
            var user = await this.userRepository
                .GetBatchAsync(x => x.Account == currentUser.Account, 1)
                .FirstOrDefaultAsync();
            return this.MapTo<TCurrentUserDto>(user);
        }

        /// <summary>
        /// 登出.
        /// </summary>
        /// <returns>操作结果.</returns>
        public virtual bool Logout()
        {
            // 不需任何操作, 前端删除 token 即可
            return true;
        }

        /// <summary>
        /// 注册.
        /// </summary>
        /// <param name="input">Register Dto.</param>
        /// <returns>注册结果.</returns>
        [AuthorizationFilter(Roles.Admin)]
        public virtual async Task<bool> RegisterAsync(TRegisterDto input)
        {
            return await this.userRepository.InsertAsync(new TUserEntity
            {
                Account = input.Account,
                Password = input.Password,
                Roles = input.Roles,
                Name = input.Name,
            });
        }

        /// <summary>
        /// 尝试用户登录.
        /// </summary>
        /// <param name="input">Login Dto.</param>
        /// <returns>登入用户实体.</returns>
        protected virtual async Task<TUserEntity> TryUserLogin(TLoginDto input)
        {
            return await this.userRepository
                .GetBatchAsync(x => x.Account == input.Account && x.Password == input.Password, 1)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 尝试管理员登陆.
        /// </summary>
        /// <param name="input">Login Dto.</param>
        /// <param name="userEntity">User Entity.</param>
        /// <returns>管理员是否登陆成功.</returns>
        protected virtual bool TryAdminLogin(TLoginDto input, out TUserEntity userEntity)
        {
            if (input.Password == this.aspireAppSettings.Administrator.Password
                && input.Account == this.aspireAppSettings.Administrator.Account)
            {
                userEntity = new TUserEntity
                {
                    Account = this.aspireAppSettings.Administrator.Account,
                    Name = this.aspireAppSettings.Administrator.Name,
                    Roles = Roles.Admin,
                };
                return true;
            }

            userEntity = default;
            return false;
        }
    }
}
