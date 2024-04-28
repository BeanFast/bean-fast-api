
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
        [HttpGet("{id}")]
        [Authorize(Utilities.Enums.RoleName.ADMIN)]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] Guid id)
        {
            var result = await _userService.GetUserResponseByIdAsync(id);
            return SuccessResult(result);
        }
        [HttpGet]
        [Authorize(Utilities.Enums.RoleName.ADMIN)]
        public async Task<IActionResult> GetAllUserAsync([FromQuery] UserFilterRequest request)
        {
            var result = await _userService.GetAllAsync(request);
            return SuccessResult(result);
        }
        [HttpPatch("{id}")]
        [Authorize(Utilities.Enums.RoleName.ADMIN)]
        public async Task<IActionResult> UpdateUserStatusAsync([FromRoute] Guid id, [FromBody] UpdateUserStatusRequest request)
        {
            await _userService.UpdateUserStatusAsync(id, request);
            return SuccessResult(new object());
        }

        [HttpPut]
        [Authorize(Utilities.Enums.RoleName.CUSTOMER)]
        public async Task<IActionResult> UpdateCustomerAsync([FromForm] UpdateCustomerRequest request)
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
        public async Task<IActionResult> CreateUserAsync([FromForm] CreateUserRequest request)
        {
            await _userService.CreateUserAsync(request);
            return SuccessResult(new object());
        }
        
    }
}
