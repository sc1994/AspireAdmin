using System.Linq;
using System.Threading.Tasks;

using Aspire.Authenticate;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Aspire
{
    internal class JwtMiddleware<TCurrentUser>
        where TCurrentUser : ICurrentUser, new()
    {
        private readonly RequestDelegate _next;
        private readonly AspireAppSettings _aspireSetupOptions;

        public JwtMiddleware(RequestDelegate next, IOptions<AspireAppSettings> aspireAppSettings)
        {
            _next = next;
            _aspireSetupOptions = aspireAppSettings.Value;
        }

        async public Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers[_aspireSetupOptions.Jwt.HeaderKey].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                try {
                    var current = new JwtManage(_aspireSetupOptions.Jwt).DeconstructionJwtToken<TCurrentUser>(token);

                    // attach user to context on successful jwt validation
                    context.Items["User"] = current;
                }
                catch {
                    // do nothing if jwt validation fails
                    // user is not attached to context so request won't have access to secure routes
                }
            await _next(context);
        }
    }
}
