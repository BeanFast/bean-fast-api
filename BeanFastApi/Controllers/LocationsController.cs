using BeanFastApi.Validators;
using DataTransferObjects.Models.Location.Request;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Utilities.Enums;

namespace BeanFastApi.Controllers
{
    public class LocationsController : BaseController
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService, IUserService userService) : base(userService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        [Authorize(RoleName.MANAGER)]
        public async Task<IActionResult> GetAllLocationAsync()
        {
            var locations = await _locationService.GetAllLocationAsync();
            return SuccessResult(locations);
        }

        [HttpGet("{id}")]
        [Authorize(RoleName.MANAGER)]
        public async Task<IActionResult> GetLocationByIdAsync([FromRoute] Guid id)
        {
            var location = await _locationService.GetByIdAsync(id);
            return SuccessResult(location);
        }
        [HttpGet("bestSellers")]
        public async Task<IActionResult> GetBestSellerLocationAsync([FromQuery] BestSellerLocationFilterRequest filterRequest)
        {
            var locations = await _locationService.GetBestSellerLocationAsync(filterRequest);
            return SuccessResult(locations);
        }
        [HttpPost]
        [Authorize(RoleName.MANAGER)]
        public async Task<IActionResult> CreateLocationAsync([FromForm] CreateLocationRequest request)
        {
            await _locationService.CreateLocationAsync(request, await GetUserAsync());
            return SuccessResult<object>(statusCode: System.Net.HttpStatusCode.Created);
        }

        [HttpPut("{id}")]
        [Authorize(RoleName.MANAGER)]
        public async Task<IActionResult> UpdateLocationAsync([FromRoute] Guid id, [FromForm] UpdateLocationRequest request)
        {
            await _locationService.UpdateLocationAsync(id, request, await GetUserAsync());
            return SuccessResult<object>();
        }

        [HttpDelete("{id}")]
        [Authorize(RoleName.MANAGER)]
        public async Task<IActionResult> DeleteLocationAsync([FromRoute] Guid id)
        {
            await _locationService.DeleteLocationAsync(id, await GetUserAsync());
            return SuccessResult<object>();
        }
    }
}
