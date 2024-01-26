using BusinessObjects.Models;
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
    [ProducesResponseType(typeof(ICollection<Food>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        ICollection<Food> foods = await _foodService.GetAllAsync();
        return SuccessResult(foods);
    }
}