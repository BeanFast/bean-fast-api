using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using System.Net;
using Utilities.Exceptions;

namespace BeanFastApi.Middlewares
{
    public class CustomAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();
        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            if (!authorizeResult.Succeeded)
            {
                
                if (authorizeResult.Challenged)
                {
                    throw new NotLoggedInOrInvalidTokenException();

                }
                else if (authorizeResult.Forbidden)
                {
                    throw new InvalidRoleException();
                }
            }
            await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}
