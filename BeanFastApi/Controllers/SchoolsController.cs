using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.School.Request;
using DataTransferObjects.Models.School.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

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
        public async Task<IPaginable<GetSchoolResponse>> GetSchoolPage(
            [FromQuery] SchoolFilterRequest filterRequest, 
            [FromQuery] PaginationRequest paginationRequest)
        {
            return await _schoolService.GetSchoolPage(paginationRequest, filterRequest);
        }
    }
}
