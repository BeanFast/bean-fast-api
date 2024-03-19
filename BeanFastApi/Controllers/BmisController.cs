using Services.Interfaces;

namespace BeanFastApi.Controllers
{
    public class BmisController : BaseController
    {
        public BmisController(IUserService userService) : base(userService)
        {
        }
    }
}
