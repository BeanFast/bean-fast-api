using BeanFastApi.Validators;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Kitchen.Request;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Formats.Tar;
using System.Net;
using Utilities.Enums;

namespace BeanFastApi.Controllers;

public class KitchensController : BaseController
{
    private readonly IKitchenService _kitchenService;

    public KitchensController(IKitchenService kitchenService, IUserService userService) : base(userService)
    {
        _kitchenService = kitchenService;
    }

    [HttpGet]
    public async Task<IActionResult> GetKitchenPageAsync(
        [FromQuery] PaginationRequest paginationRequest,
        [FromQuery] KitchenFilterRequest filterRequest)
    {
        string? userRole = GetUserRole();
        object kitchens = paginationRequest is { Size: 0, Page: 0 }
            ? await _kitchenService.GetAllAsync(userRole, filterRequest)
            : await _kitchenService.GetKitchenPageAsync(paginationRequest, filterRequest, userRole);
        return SuccessResult(kitchens);
    }
    [HttpGet("current")]
    [Authorize(RoleName.MANAGER)]
    public async Task<IActionResult> GetKitchenByCurrentManagerAsync()
    {
        var result = await _kitchenService.GetKitchenByCurrentManagerAsync(await GetUserAsync());
        return SuccessResult(result);
    }
    [HttpGet("{kitchenId}/schools/count")]
    public async Task<IActionResult> CountSchoolByKitchenIdAsync([FromRoute] Guid kitchenId)
    {
        return SuccessResult(await _kitchenService.CountSchoolByKitchenIdAsync(kitchenId));
    }
    [HttpPost]
    public async Task<IActionResult> CreateKitchenAsync([FromForm] CreateKitchenRequest request)
    {
        await _kitchenService.CreateKitchenAsync(request, await GetUserAsync());
        return SuccessResult<object>(statusCode: HttpStatusCode.Created, data: new { Id = Guid.NewGuid() });
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateKitchenAsync([FromRoute] Guid id, [FromForm] CreateKitchenRequest request)
    {
        return null;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteKitchenAsync([FromRoute] Guid id)
    {
        await _kitchenService.DeleteKitchenAsync(id, await GetUserAsync());
        return SuccessResult<object>(statusCode: HttpStatusCode.OK);
    }
}