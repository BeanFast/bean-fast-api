
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
        
        [HttpPost]
        [Authorize(Utilities.Enums.RoleName.ADMIN)]
        public async Task<IActionResult> CreateUser([FromForm] CreateUserRequest request)
        {
            await _userService.CreateUserAsync(request);
            return SuccessResult(new object());
        }
    }
}
