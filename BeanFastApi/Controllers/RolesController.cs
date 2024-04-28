using BeanFastApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace BeanFastApi.Controllers
{
    public class RolesController : BaseController
    {
        private readonly IRoleService _roleService;
        public RolesController(IUserService userService, IRoleService roleService) : base(userService)
        {
            _roleService = roleService;
        }
        [HttpGet]
        [Authorize(Utilities.Enums.RoleName.ADMIN)]
        public async Task<IActionResult> GetRolesAsync()
        {
            var roles = await _roleService.GetAllAsync();
            return SuccessResult(roles);
        }
    }
}
