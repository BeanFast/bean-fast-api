using DataTransferObjects.Core.Response;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

namespace BeanFastApi.Controllers
{
    [Route(ApiEndpointConstants.ApiEndpoint + "/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult SuccessResult(object data, string? code = null, string? message = null)
        {
            var response = new SuccessApiResponse
            {
                Data = data,
                Code = code ?? CodeContants.DefaultApiCodeContants.ApiSuccess,
                Message = message ?? MessageContants.DefaultApiMessage.ApiSuccess,
            };
            return Ok(response);
        }
    }
}
