using BeanFastApi.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeanFastApi.Controllers
{
    [Route(ApiEndpointContants.ApiEndpoint + "/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
    }
}
