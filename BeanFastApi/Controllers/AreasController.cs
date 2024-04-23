using DataTransferObjects.Models.Area.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Services.Interfaces;

namespace BeanFastApi.Controllers
{
    public class AreasController : BaseController
    {
        private readonly IAreaService _areaService;

        public AreasController(IAreaService areaService, IUserService userService) : base(userService)
        {
            _areaService = areaService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAreaAsync([FromQuery] AreaFilterRequest request)
        {
            return SuccessResult(await _areaService.SearchAreaAsync(request));
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            return SuccessResult(await _areaService.GetAllAsync());
        }
        [HttpGet("cities")]
        public async Task<IActionResult> GetCityNamesAsync([FromQuery] string name = "")
        {
            return SuccessResult(await _areaService.SearchCityNamesAsync(name));
        }

        [HttpGet("cities/{cityName}/districts")]
        public async Task<IActionResult> GetDistrictNamesAsync([FromRoute] string cityName, [FromQuery] string name = "")
        {
            return SuccessResult(await _areaService.SearchDistrictNamesAsync(cityName, name));
        }

        [HttpGet("cities/{cityName}/districts/{districtName}/wards")]
        public async Task<IActionResult> GetWardNamesAsync([FromRoute] string cityName, [FromRoute] string districtName, [FromQuery] string name = "")
        {
            return SuccessResult(await _areaService.SearchWardNamesAsync(cityName, districtName, name));
        }
    }
}