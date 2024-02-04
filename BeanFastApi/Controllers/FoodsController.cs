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

namespace BeanFastApi.Controllers;

public class FoodsController : BaseController
{
    private readonly IFoodService _foodService;

    public FoodsController(IFoodService foodService)
    {
        _foodService = foodService;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(SuccessApiResponse<IPaginable<GetFoodResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPageAsync([FromQuery] PaginationRequest request)
    {
        object foods;
        if (request.Size == 0 && request.Page == 0)
        {
            foods = await _foodService.GetAllAsync();
        }
        else
        {
            foods = await _foodService.GetPageAsync(request);
        }
        return SuccessResult(foods);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Food), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromRoute]Guid id)
    {
        GetFoodResponse food = await _foodService.GetFoodResponseByIdAsync(id);
        return SuccessResult(food);
    }
    [HttpPost]
    public async Task <IActionResult> CreateFood([FromForm] CreateFoodRequest request)
    {
        await _foodService.CreateFoodAsync(request);
        return SuccessResult<object>(statusCode: System.Net.HttpStatusCode.Created);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFood([FromRoute] Guid id)
    {
        await _foodService.DeleteAsync(id);
        return SuccessResult<object>(statusCode: HttpStatusCode.OK);
    }
}