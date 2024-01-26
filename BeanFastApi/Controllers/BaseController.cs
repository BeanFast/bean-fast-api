using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

namespace BeanFastApi.Controllers
{
    [Route(ApiEndpointConstants.ApiEndpoint + "/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
    }
}
