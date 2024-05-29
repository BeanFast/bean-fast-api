using System.Net;
using Utilities.Settings;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Core.Response;
using DataTransferObjects.Models.Food.Request;
using DataTransferObjects.Models.Food.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services.Interfaces;
using BeanFastApi.Validators;
using Utilities.Enums;

namespace BeanFastApi.Controllers;

public class FoodsController : BaseController
{
    private readonly IFoodService _foodService;

    public FoodsController(IFoodService foodService, IUserService userService) : base(userService)
    {
        _foodService = foodService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(SuccessApiResponse<IPaginable<GetFoodResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPageAsync(
        [FromQuery] FoodFilterRequest filterRequest,
        [FromQuery] PaginationRequest paginationRequest)
    {
        object foods;
        var userRole = GetUserRole();
        if (paginationRequest is { Size: 0, Page: 0 })
        {
            foods = await _foodService.GetAllAsync(userRole, filterRequest);
        }
        else
        {
            foods = await _foodService.GetPageAsync(userRole, filterRequest, paginationRequest);
        }

        return SuccessResult(foods);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Food), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        GetFoodResponse food = await _foodService.GetFoodResponseByIdAsync(id);
        return SuccessResult(food);
    }
    [Authorize(RoleName.MANAGER)]
    [HttpGet("bestSellers")]
    public async Task<IActionResult> GetBestSellerFoodsAsync([FromQuery] GetBestSellerFoodsRequest request)
    {
        var data = await _foodService.GetBestSellerFoodsAsync(request, await GetManagerAsync());
        return SuccessResult(data);
    }
    [HttpPost]
    [Authorize(RoleName.MANAGER)]
    public async Task<IActionResult> CreateFoodAsync([FromForm] CreateFoodRequest request)
    {
        await _foodService.CreateFoodAsync(request, await GetUserAsync());
        return SuccessResult<object>(statusCode: HttpStatusCode.Created);
    }

    [HttpPut("{id}")]
    [Authorize(RoleName.MANAGER)]
    public async Task<IActionResult> UpdateFoodAsync([FromRoute] Guid id, [FromForm] UpdateFoodRequest request)
    {
        await _foodService.UpdateFoodAsync(id, request, await GetUserAsync());
        return SuccessResult<object>(statusCode: HttpStatusCode.OK);
    }

    [HttpDelete("{id}")]
    [Authorize(RoleName.MANAGER)]
    public async Task<IActionResult> DeleteFoodAsync([FromRoute] Guid id)
    {
        await _foodService.DeleteAsync(id, await GetUserAsync());
        return SuccessResult<object>(statusCode: HttpStatusCode.OK);
    }
}