using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

namespace Aspire.Core.Authenticate
{
    internal class JwtManage
    {
        private readonly JwtOptions _jwtOptions;

        public JwtManage(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }

        public string GenerateJwtToken(ICurrentUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(nameof(ICurrentUser.Account), user.Account),
                    new Claim(nameof(ICurrentUser.Name), user.Name)
                }),
                Expires = DateTime.Now.AddSeconds(_jwtOptions.ExpireSeconds),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ICurrentUser DeconstructionJwtToken<TUserEntity, TPrimaryKey>(string jwtToken)
            where TUserEntity : IUserEntity<TPrimaryKey>, new()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            tokenHandler.ValidateToken(jwtToken, new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clocks kew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var token = (JwtSecurityToken)validatedToken;
            var userId = token.Claims.First(x => x.Type == nameof(ICurrentUser.Account)).Value;
            var name = token.Claims.First(x => x.Type == nameof(ICurrentUser.Name)).Value;
            return new TUserEntity {
                Account = userId,
                Name = name
            };
        }
    }
}