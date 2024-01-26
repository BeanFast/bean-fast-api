using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Food.Response;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace BeanFastApi.Controllers;

public class FoodsController : BaseController
{
    private readonly IFoodService _foodService;

    public FoodsController(IFoodService foodService)
    {
        _foodService = foodService;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IPaginable<GetFoodResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPageAsync([FromQuery] PaginationRequest request)
    {
        IPaginable<GetFoodResponse> foods = await _foodService.GetPageAsync(request);
        return SuccessResult(foods);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Food), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromRoute]Guid id)
    {
        GetFoodResponse food = await _foodService.GetByIdAsync(id);
        return SuccessResult(food);
    }
    
}