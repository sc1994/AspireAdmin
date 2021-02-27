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
                Subject = new ClaimsIdentity(typeof(ICurrentUser).GetProperties()
                    .Select(x => new Claim(x.Name, x.GetValue(user)?.ToString() ?? string.Empty))
                    .ToArray()),
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
            return new TCurrentUser {
                Account = token.Claims.First(x => x.Type == nameof(ICurrentUser.Account)).Value,
                Name = token.Claims.First(x => x.Type == nameof(ICurrentUser.Name)).Value,
                Roles = token.Claims.First(x => x.Type == nameof(ICurrentUser.Roles)).Value,
            };
        }
    }
}