using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace KeePassShtokal.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        // Dependency Injection
        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //Reading the AuthHeader which is signed with JWT
            
            

            //Pass to the next middleware
            await _next(context);
        }
    }
}
