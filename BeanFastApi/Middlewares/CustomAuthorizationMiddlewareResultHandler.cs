using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace BeanFastApi.Middlewares
{
    public class CustomAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();
        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            if (!authorizeResult.Succeeded)
            {
                policy.Requirements.ToList().ForEach(r => Console.WriteLine(r));
                var response = context.Response;
                object returnedData = new { };
                
                int statusCode = 401;
                if (authorizeResult.Challenged)
                {
                    returnedData = new { Message = "You are not logged in or access token is not valid" };
                }
                else if (authorizeResult.Forbidden)
                {
                    returnedData = new { Message = "You are not allowed for this feature" };
                }
                response.StatusCode = statusCode;
                response.ContentType = "application/json";
                _ = response.WriteAsJsonAsync(returnedData);


                return;
            }
            await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}
