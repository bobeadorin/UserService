﻿
using UserService.Constant;
using Microsoft.AspNetCore.Authorization;

namespace UserService.Middleware
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
                    var refreshToken = context.Request.Cookies[CookieConfig.RefreshToken];


                    if (!string.IsNullOrEmpty(token) && String.IsNullOrEmpty(context.Request.Headers["Authorization"]))
                    {
                        context.Request.Headers.Add("Authorization", $"Bearer {token}");
                    }
                }
           }
            
            await _next(context);
        }
    }
}
