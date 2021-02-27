using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

namespace Aspire.Authenticate
{
    internal class JwtManage
    {
        private readonly JwtAppSettings _jwtAppSettings;

        public JwtManage(JwtAppSettings jwtAppSettings)
        {
            _jwtAppSettings = jwtAppSettings;
        }

        public string GenerateJwtToken(ICurrentUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtAppSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(nameof(ICurrentUser.Account), user.Account),
                    new Claim(nameof(ICurrentUser.Name), user.Name)
                }),
                Expires = DateTime.Now.AddSeconds(_jwtAppSettings.ExpireSeconds),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ICurrentUser DeconstructionJwtToken<TCurrentUser>(string jwtToken)
            where TCurrentUser : ICurrentUser, new()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtAppSettings.Secret);
            tokenHandler.ValidateToken(jwtToken, new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clocks kew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                // TODO what?
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var token = (JwtSecurityToken)validatedToken;
            var userId = token.Claims.First(x => x.Type == nameof(ICurrentUser.Account)).Value;
            var name = token.Claims.First(x => x.Type == nameof(ICurrentUser.Name)).Value;
            return new TCurrentUser {
                Account = userId,
                Name = name
            };
        }
    }
}