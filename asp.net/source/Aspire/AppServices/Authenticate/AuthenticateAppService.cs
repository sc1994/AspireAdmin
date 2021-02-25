using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Aspire.Core;

using Microsoft.IdentityModel.Tokens;


namespace Aspire.AppServices.Authenticate
{
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
        public async Task<string> LoginAsync(LoginInputDto input)
        {
            TUserEntity user;

            if (input.Password == _aspireConfigureOptions.Administrator.Password
                && input.UserId == _aspireConfigureOptions.Administrator.UserId) {
                user = new TUserEntity {
                    Id = (TPrimaryKey)Convert.ChangeType(_aspireConfigureOptions.Administrator.PrimaryKey, typeof(TPrimaryKey)),
                    UserId = _aspireConfigureOptions.Administrator.UserId,
                    Name = _aspireConfigureOptions.Administrator.Name
                };
            }
            else {
                user = await GetUserByIdAndPwdAsync(input);
            }

            if (user is not null) {
                return GenerateJwtToken(user);
            }

            throw new Exception("登录失败");
        }

        private async Task<TUserEntity> GetUserByIdAndPwdAsync(LoginInputDto input)
        {
            return await ServiceLocator.ServiceProvider.GetService<IAuditRepository<TUserEntity, TPrimaryKey>>()
                 .GetBatchAsync(x => x.UserId == input.UserId && x.Password == input.Password)
                 .FirstOrDefaultAsync();
        }

        private string GenerateJwtToken(ICurrentUser user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_aspireConfigureOptions.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[] {
                new Claim("userId", user.UserId),
                new Claim("username", user.Name)
            }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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
    }
}
