using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Kitchen.Request;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Formats.Tar;
using System.Net;

namespace BeanFastApi.Controllers;

public class KitchensController : BaseController
{
    private readonly IKitchenService _kitchenService;

    public KitchensController(IKitchenService kitchenService)
    {
        _kitchenService = kitchenService;
    }

    [HttpGet]
    public async Task<IActionResult> GetKitchenPageAsync(
        [FromQuery] PaginationRequest paginationRequest,
        [FromQuery] KitchenFilterRequest filterRequest)
    {
        string? userRole = this.GetUserRole();
        return SuccessResult(await _kitchenService.GetKitchenPageAsync(paginationRequest, filterRequest, userRole));
    }
    [HttpPost]
    public async Task<IActionResult> CreateKitchenAsync([FromForm] CreateKitchenRequest request)
    {
        await _kitchenService.CreateKitchenAsync(request);
        return SuccessResult<object>(statusCode: HttpStatusCode.Created, data: new { Id = Guid.NewGuid() });
    }
    [HttpPut]
    public async Task<IActionResult> UpdateKitchenAsync([FromForm] CreateKitchenRequest request)
    {
        return null;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteKitchenAsync([FromRoute] Guid id)
    {
        await _kitchenService.DeleteKitchenAsync(id);
        return SuccessResult<object>(statusCode: HttpStatusCode.OK);
    }
}