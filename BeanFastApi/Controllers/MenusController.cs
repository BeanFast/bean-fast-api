using System.Security.Claims;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using Microsoft.AspNetCore.Mvc;
using Services.Implements;
using Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using DataTransferObjects.Models.Menu.Response;
using DataTransferObjects.Models.Menu.Request;
using BeanFastApi.Validators;
namespace BeanFastApi.Controllers;

public class MenusController : BaseController
{
    private readonly IMenuService _menuService;
    public MenusController(IMenuService menuService, IUserService userService) : base(userService)
    {
        _menuService = menuService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMenusAsync(
        [FromQuery] PaginationRequest paginationRequest,
        [FromQuery] MenuFilterRequest filterRequest
        )
    {
        object menus;
        var userRole = GetUserRole();
        if (paginationRequest is { Size: 0, Page: 0 })
        {
            menus = await _menuService.GetAllAsync(userRole, filterRequest);
        }
        else
        {
            menus = await _menuService.GetPageAsync(paginationRequest, userRole, filterRequest);
        }

        return SuccessResult(menus);
        //return Problem()
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMenuByIdAsync([FromRoute] Guid id)
    {
        var menu = await _menuService.GetGetMenuResponseByIdAsync(id);
        return SuccessResult(menu);
    }
    [HttpPost]
    [Authorize(Utilities.Enums.RoleName.MANAGER)]
    public async Task<IActionResult> CreateMenuAsync([FromBody] CreateMenuRequest request)
    {
        await _menuService.CreateMenuAsync(request, GetUserId());
        return SuccessResult<object>();
    }
    [HttpPut("{id}")]
    [Authorize(Utilities.Enums.RoleName.MANAGER)]
    public async Task<IActionResult> UpdateMenuAsync([FromRoute] Guid id, [FromBody] UpdateMenuRequest request)
    {
        await _menuService.UpdateMenuAsync(request, id, await GetUserAsync());
        return SuccessResult<object>();
    }
    [HttpDelete("{id}")]
    [Authorize(Utilities.Enums.RoleName.MANAGER)]
    public async Task<IActionResult> DeleteMenuAsync([FromRoute] Guid id)
    {

        await _menuService.DeleteMenuAsync(id);

        return SuccessResult<object>();
    }
}