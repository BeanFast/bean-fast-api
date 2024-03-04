using DataTransferObjects.Models.Location.Request;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace BeanFastApi.Controllers
{
    public class LocationsController : BaseController
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        

        //[HttpPost]
        //public async Task<IActionResult> CreateLocationAsync([FromForm] CreateLocationRequest request)
        //{
        //    await _locationService.CreateLocationAsync(request);
        //    return  SuccessResult<object>(statusCode: System.Net.HttpStatusCode.Created);
        //}
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateLocationAsync([FromRoute] Guid id, [FromForm] UpdateLocationRequest request)
        //{
        //    await _locationService.UpdateLocationAsync(id, request);
        //    return SuccessResult<object>();
        //}
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteLocationAsync([FromRoute]Guid id)
        //{
        //    await _locationService.DeleteLocationAsync(id);
        //    return SuccessResult<object>();
        //}   
    }
}
