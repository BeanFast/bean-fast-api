using Services.Interfaces;

namespace BeanFastApi.Controllers
{
    public class LoyaltyCardsController : BaseController
    {
        public LoyaltyCardsController(IUserService userService) : base(userService)
        {
        }
    }
}
