
using System.Runtime.CompilerServices;
using UserService.Constant;
using Microsoft.AspNetCore.Authorization;

namespace UserService.MIddleware
{
    public class TokenCookieMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
           var endpoint = context.GetEndpoint();

           if (endpoint != null)
           {
               var authorizeAttribute = endpoint.Metadata.GetMetadata<AuthorizeAttribute>();

                if (authorizeAttribute != null && !context.Request.Headers.ContainsKey("Authorization"))
                {
                    var token = context.Request.Cookies[CookieConfig.AccessToken];

                    if (!string.IsNullOrEmpty(token))
                    {
                        context.Request.Headers.Add("Authorization", $"Bearer {token}");
                    }
                }
           }
            await _next(context);
        }
    }
}
