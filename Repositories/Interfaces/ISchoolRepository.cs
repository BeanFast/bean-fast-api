using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.School.Request;
using DataTransferObjects.Models.School.Response;
using System.Threading.Tasks;

namespace Repositories.Interfaces;

public interface ISchoolRepository : IGenericRepository<School>
{
    Task<IPaginable<GetSchoolIncludeAreaAndLocationResponse>> GetSchoolPageAsync(PaginationRequest paginationRequest, SchoolFilterRequest filterRequest);
    Task<int> CountStudentAsync(Guid schoolId);
    Task<School> GetByIdIncludeProfile(Guid schoolId);
    Task<ICollection<GetSchoolIncludeAreaAndLocationResponse>> GetSchoolListAsync(PaginationRequest paginationRequest, SchoolFilterRequest filterRequest);
    Task<School> GetSchoolByIdAsync(Guid id);
    Task<School> GetSchoolByIdAsync(int status, Guid id);
    Task<School?> GetSchoolByAreaIdAndAddress(Guid areaId, string address);
}