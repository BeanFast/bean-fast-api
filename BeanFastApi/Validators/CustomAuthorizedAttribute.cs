using BusinessObjects.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Validators
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorizedAttribute : AuthorizeAttribute
    {
        public CustomAuthorizedAttribute(params RoleName[] roles)
        {
            var allowedRolesAsString = roles.Select(x => x.GetDisplayName());
            Roles = string.Join(",", allowedRolesAsString);

        }

        //override  
    }
}
