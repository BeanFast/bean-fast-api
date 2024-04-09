
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
        [HttpGet("deliverers")]
        [Authorize(Utilities.Enums.RoleName.MANAGER)]
        public async Task<IActionResult> GetAllAvailableDeliverer()
        {
            var deliverers = await _userService.GetAvailableDeliverersAsync();
            return SuccessResult(deliverers);
        }
        
        [HttpPut]
        [Authorize(Utilities.Enums.RoleName.CUSTOMER)]
        public async Task<IActionResult> UpdateCustomer([FromForm] UpdateCustomerRequest request)
        {
            await _userService.UpdateCustomerAsync(request, await GetUserAsync());
            return SuccessResult<object>(new object());
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
