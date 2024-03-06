using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.School.Request;
using DataTransferObjects.Models.School.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Net;

namespace BeanFastApi.Controllers
{
    public class SchoolsController : BaseController
    {
        private readonly ISchoolService _schoolService;

        public SchoolsController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSchoolPageAsync(
            [FromQuery] SchoolFilterRequest filterRequest, 
            [FromQuery] PaginationRequest paginationRequest)
        {
            return SuccessResult(await _schoolService.GetSchoolListAsync(paginationRequest, filterRequest));
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchoolAsync([FromForm] CreateSchoolRequest request)
        {
            await _schoolService.CreateSchoolAsync(request);
            return  SuccessResult<object>(statusCode: HttpStatusCode.Created);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchoolAsync([FromRoute] Guid id, [FromForm] UpdateSchoolRequest request)
        {
            await _schoolService.UpdateSchoolAsync(id, request);
            return SuccessResult<object>();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchoolAsync([FromRoute]Guid id)
        {
            await _schoolService.DeleteSchoolAsync(id);
            return SuccessResult<object>();
        }
    }
}
