
using BeanFastApi.Validators;
using DataTransferObjects.Models.User.Request;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace BeanFastApi.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(IUserService userService) : base(userService)
        {

        }
        
        
        [HttpPut]
        [Authorize(Utilities.Enums.RoleName.CUSTOMER)]
        public async Task<IActionResult> UpdateCustomer([FromForm] UpdateCustomerRequest request)
        {
            await _userService.UpdateCustomerAsync(request, await GetUserAsync());
            return SuccessResult<object>(new object());
        }
        [HttpPut("qrCode")]
        [Authorize(Utilities.Enums.RoleName.CUSTOMER)]
        public async Task<IActionResult> GenerateQrCodeAsync()
        {
            var qrcode = await _userService.GenerateQrCodeAsync(await GetUserAsync());
            return SuccessResult(qrcode);
        }
        [HttpPost]
        [Authorize(Utilities.Enums.RoleName.ADMIN)]
        public async Task<IActionResult> CreateUser([FromForm] CreateUserRequest request)
        {
            await _userService.CreateUserAsync(request);
            return SuccessResult(new object());
        }
    }
}
