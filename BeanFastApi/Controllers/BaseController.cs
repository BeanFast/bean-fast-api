using DataTransferObjects.Core.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Utilities.Constants;

namespace BeanFastApi.Controllers
{
    [Route(ApiEndpointConstants.ApiEndpoint + "/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult SuccessResult(object data, string? code = null, string? message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var response = new SuccessApiResponse
            {
                Data = data,
                Code = code ?? CodeContants.DefaultApiCodeContants.ApiSuccess,
                Message = message ?? MessageContants.DefaultApiMessage.ApiSuccess,
            };
            
            return new ObjectResult(response) { StatusCode = (int) statusCode };
        }
    }
}
