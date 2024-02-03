using DataTransferObjects.Core.Pagination;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace BeanFastApi.Controllers;

public class KitchensController : BaseController
{
    private readonly IKitchenService _kitchenService;

    public KitchensController(IKitchenService kitchenService)
    {
        _kitchenService = kitchenService;
    }

    [HttpGet]
    public async Task<IActionResult> GetKitchenPage([FromQuery] PaginationRequest paginationRequest)
    {
        return SuccessResult(await _kitchenService.GetKitchenPage(paginationRequest));
    }
}