using BeanFastApi.Validators;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.School.Request;
using DataTransferObjects.Models.School.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Net;
using Utilities.Enums;

namespace BeanFastApi.Controllers
{
    public class SchoolsController : BaseController
    {
        private readonly ISchoolService _schoolService;

        public SchoolsController(ISchoolService schoolService, IUserService userService) : base(userService)
        {
            _schoolService = schoolService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSchoolPageAsync(
            [FromQuery] SchoolFilterRequest filterRequest,
            [FromQuery] PaginationRequest paginationRequest)
        {
            User user = null;
            if (GetUserRole() == RoleName.MANAGER.ToString())
            {
                user = await GetManagerAsync();
            }
            else
            {
                user = await GetUserAsync();
            }
            var result = await _schoolService.GetSchoolListAsync(paginationRequest, filterRequest, user);
            return SuccessResult(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchoolByIdAsync(
            [FromRoute] Guid id)
        {
            var result = await _schoolService.GetSchoolIncludeAreaAndLocationResponseByIdAsync(id);
            return SuccessResult(result);
        }
        [HttpGet("{schooldId}/students/count")]
        public async Task<IActionResult> CountStudentAsync([FromRoute] Guid schooldId)
        {
            return SuccessResult(await _schoolService.CountStudentAsync(schooldId));
        }

        [HttpPost]
        [Authorize(Utilities.Enums.RoleName.MANAGER)]

        public async Task<IActionResult> CreateSchoolAsync([FromForm] CreateSchoolRequest request)
        {
            await _schoolService.CreateSchoolAsync(request, await GetManagerAsync());
            return SuccessResult<object>(statusCode: HttpStatusCode.Created);
        }
        [HttpPut("{id}")]
        [Authorize(Utilities.Enums.RoleName.MANAGER)]
        public async Task<IActionResult> UpdateSchoolAsync([FromRoute] Guid id, [FromForm] UpdateSchoolRequest request)
        {
            await _schoolService.UpdateSchoolAsync(id, request, await GetManagerAsync());
            return SuccessResult<object>();
        }
        [HttpDelete("{id}")]
        [Authorize(Utilities.Enums.RoleName.MANAGER)]

        public async Task<IActionResult> DeleteSchoolAsync([FromRoute] Guid id)
        {
            await _schoolService.DeleteSchoolAsync(id, await GetManagerAsync());
            return SuccessResult<object>();
        }
    }
}
