using BeanFastApi.Extensions;
using BusinessObjects.Models;
using DataTransferObjects.Core.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System.Net;
using System.Security.Claims;
using Utilities.Constants;
using Utilities.Exceptions;
using Utilities.Settings;

namespace BeanFastApi.Controllers
{
    [Route(ApiEndpointConstants.ApiEndpoint + "/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IUserService _userService;

        public BaseController(IUserService userService)
        {
            _userService = userService;
        }

        protected IActionResult SuccessResult<T>(T? data = default, string? message = null,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var response = new SuccessApiResponse<T>
            {
                Data = data,
                Message = message ?? MessageConstants.DefaultMessageConstrant.ApiSuccess,
            };

            return new ObjectResult(response) { StatusCode = (int)statusCode };
        }
        protected async Task<User> GetUserAsync()
        {
            string? userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdStr.IsNullOrEmpty()) throw new NotLoggedInOrInvalidTokenException();
            return await _userService.GetByIdAsync(Guid.Parse(userIdStr!));
            //string? 
        }

        protected string? GetUserRole()
        {
            return User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        }


        protected Guid GetUserId()
        {
            string? userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdStr!);
        }
    }
}