using BeanFastApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Utilities.Enums;

namespace BeanFastApi.Controllers
{
    public class WalletsController : BaseController
    {
        private readonly IWalletService _walletService;
        public WalletsController(IWalletService walletService, IUserService userService) : base(userService)
        {
            _walletService = walletService;
        }
        [Authorize(RoleName.CUSTOMER)]
        [HttpGet("{profileId}")]
        public async Task <IActionResult> GetWalletsByCurrentCustomerAndProfileAsync([FromRoute] Guid profileId)
        {
            var customerId = GetUserId();
            var result = await _walletService.GetWalletByCurrentCustomerAndProfileAsync(customerId, profileId);
            return SuccessResult(result);   
        }
        
    }
}
