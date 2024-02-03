using DataTransferObjects.Core.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Utilities.Constants;

namespace BeanFastApi.Controllers
{
    [Route(ApiEndpointConstants.ApiEndpoint + "/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult SuccessResult(object? data = null, string? message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var response = new SuccessApiResponse
            {
                Data = data,
                Message = message ?? MessageContants.DefaultApiMessage.ApiSuccess,
            };
            
            return new ObjectResult(response) { StatusCode = (int) statusCode };
        }


        protected string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        }
    }
}
