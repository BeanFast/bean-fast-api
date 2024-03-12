using DataTransferObjects.Core.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Claims;
using Utilities.Constants;
using Utilities.Settings;

namespace BeanFastApi.Controllers
{
    [Route(ApiEndpointConstants.ApiEndpoint + "/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
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