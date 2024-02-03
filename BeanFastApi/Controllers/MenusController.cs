using System.Security.Claims;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using Microsoft.AspNetCore.Mvc;
using Services.Implements;
using Services.Interfaces;
using Microsoft.AspNetCore.Identity;
namespace BeanFastApi.Controllers;

public class MenusController : BaseController
{
    private readonly IMenuService _menuService;
    public MenusController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet]
    public async Task<IActionResult> GetFoodPage([FromQuery] PaginationRequest paginationRequest)
    {
        string userId = GetUserId();
        Console.WriteLine("User id: ", userId);
        return SuccessResult(await _menuService.GetMenuPage(paginationRequest));
    }
}