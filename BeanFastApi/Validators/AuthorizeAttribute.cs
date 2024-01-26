using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Extensions;
using Utilities.Enums;

namespace BeanFastApi.Validators
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
    {
        public AuthorizeAttribute(params RoleName[] roles)
        {
            var allowedRolesAsString = roles.Select(x => x.GetDisplayName());
            Roles = string.Join(",", allowedRolesAsString);
        }

    }
}
