// <copyright file="JwtMiddleware.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System.Linq;
    using System.Threading.Tasks;
    using Aspire.Authenticate;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Jwt Middleware.
    /// </summary>
    /// <typeparam name="TCurrentUser">Current User.</typeparam>
    internal class JwtMiddleware<TCurrentUser>
        where TCurrentUser : ICurrentUser, new()
    {
        private readonly RequestDelegate next;
        private readonly AspireAppSettings aspireSetupOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtMiddleware{TCurrentUser}"/> class.
        /// </summary>
        /// <param name="next">Next Middleware.</param>
        /// <param name="aspireAppSettings">App Settings.</param>
        public JwtMiddleware(RequestDelegate next, IOptions<AspireAppSettings> aspireAppSettings)
        {
            this.next = next;
            this.aspireSetupOptions = aspireAppSettings.Value;
        }

        /// <summary>
        /// Invoke.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <returns>Task.</returns>
        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers[this.aspireSetupOptions.Jwt.HeaderKey].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                try
                {
                    var current = new JwtManage(this.aspireSetupOptions.Jwt).DeconstructionJwtToken<TCurrentUser>(token);

                    // attach user to context on successful jwt validation
                    context.Items[ICurrentUser.HttpItemsKey] = current;
                }
                catch
                {
                    // do nothing if jwt validation fails
                    // user is not attached to context so request won't have access to secure routes
                }
            }

            await this.next(context);
        }
    }
}
