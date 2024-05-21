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
        [HttpGet("")]
        public async Task <IActionResult> GetWalletsByCurrentCustomerAndProfileAsync()
        {
            var customerId = GetUserId();
            var result = await _walletService.GetWalletByCurrentCustomerAndProfileAsync(customerId);
            return SuccessResult(result);   
        }
        
    }
}
