using System.Security.Claims;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using Microsoft.AspNetCore.Mvc;
using Services.Implements;
using Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using DataTransferObjects.Models.Menu.Response;
using DataTransferObjects.Models.Menu.Request;
namespace BeanFastApi.Controllers;

public class MenusController : BaseController
{
    private readonly IMenuService _menuService;
    public MenusController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMenusAsync(
        [FromQuery] PaginationRequest paginationRequest,
        [FromQuery] MenuFilterRequest filterRequest
        )
    {
        object foods;
        var userRole = GetUserRole();
        if (paginationRequest is { Size: 0, Page: 0 })
        {
            foods = await _menuService.GetAllAsync(userRole, filterRequest);
        }
        else
        {
            foods = await _menuService.GetPageAsync(paginationRequest, userRole, filterRequest);
        }

        return SuccessResult(foods);
        //return Problem()
    }
}