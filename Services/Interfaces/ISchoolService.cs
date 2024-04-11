using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.School.Request;
using DataTransferObjects.Models.School.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISchoolService : IBaseService
    {
        Task CreateSchoolAsync(CreateSchoolRequest request, User user);
        Task<IPaginable<GetSchoolIncludeAreaAndLocationResponse>> GetSchoolPageAsync(PaginationRequest paginationRequest, SchoolFilterRequest filterRequest);
        Task<ICollection<GetSchoolIncludeAreaAndLocationResponse>> GetSchoolListAsync(PaginationRequest paginationRequest, SchoolFilterRequest filterRequest);
        Task<School> GetSchoolByIdAsync(int status, Guid id);
        Task<School> GetSchoolByIdAsync(Guid id);
        Task<GetSchoolIncludeAreaAndLocationResponse> GetSchoolIncludeAreaAndLocationResponseByIdAsync(Guid id);
        Task DeleteSchoolAsync(Guid id, User user);
        Task UpdateSchoolAsync(Guid id, UpdateSchoolRequest request, User user);
        Task<int> CountStudentAsync(Guid schoolId);
    }
}
