using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.School.Request;
using DataTransferObjects.Models.School.Response;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Linq.Expressions;
using Utilities.Constants;
using Utilities.Exceptions;
using Utilities.Statuses;

namespace Repositories.Implements;

public class SchoolRepository : GenericRepository<School>, ISchoolRepository
{
    public SchoolRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {
    }
    private List<Expression<Func<School, bool>>> GetSchoolFilterFromFilterRequest(SchoolFilterRequest filterRequest)
    {
        List<Expression<Func<School, bool>>> filters = new();
        if (filterRequest.Code != null)
        {
            filters.Add(s => s.Code == filterRequest.Code);
        }
        if (filterRequest.Name != null)
        {
            filters.Add(s => s.Name.ToLower().Contains(filterRequest.Name.ToLower()));
        }
        if (filterRequest.Address != null)
        {
            filters.Add(s => s.Address.ToLower().Contains(filterRequest.Address.ToLower()));
        }

        return filters;
    }
    public async Task<IPaginable<GetSchoolIncludeAreaAndLocationResponse>> GetSchoolPageAsync(PaginationRequest paginationRequest, SchoolFilterRequest filterRequest)
    {
        var filters = GetSchoolFilterFromFilterRequest(filterRequest);
        var page = await GetPageAsync<GetSchoolIncludeAreaAndLocationResponse>(
                filters: filters,
                paginationRequest: paginationRequest,
                include: s => s.Include(s => s.Area).Include(school => school.Locations!)
            );
        return page;
    }
    public async Task<ICollection<GetSchoolIncludeAreaAndLocationResponse>> GetSchoolListAsync(PaginationRequest paginationRequest, SchoolFilterRequest filterRequest)
    {
        var filters = GetSchoolFilterFromFilterRequest(filterRequest);
        filters.Add(s => s.Status != BaseEntityStatus.Deleted);
        var result = await GetListAsync<GetSchoolIncludeAreaAndLocationResponse>(
            filters: filters,
            include: s => s.Include(s => s.Area).Include(s => s.Locations!.Where(l => l.Status == BaseEntityStatus.Active))
        );
        foreach (var item in result)
        {
            item.StudentCount = await CountStudentAsync(item.Id);
        }
        return result;
    }
    public async Task<int> CountStudentAsync(Guid schoolId)
    {
        var school = await GetByIdIncludeProfile(schoolId);
        return school.Profiles!.Count();
    }
    public async Task<School> GetByIdIncludeProfile(Guid schoolId)
    {
        var school = await FirstOrDefaultAsync(filters: new()
            {
                s => s.Id == schoolId 
            }, include: i => i.Include(s => s.Profiles!)) ?? throw new EntityNotFoundException(MessageConstants.SchoolMessageConstrant.SchoolNotFound(schoolId));
        return school;
    }
    public async Task<School> GetSchoolByIdAsync(Guid id)
    {
        var school = await FirstOrDefaultAsync(filters: new()
            {
                s => s.Id == id
            }) ?? throw new EntityNotFoundException(MessageConstants.SchoolMessageConstrant.SchoolNotFound(id));
        return school;
    }
    public async Task<School> GetSchoolByIdAsync(int status, Guid id)
    {
        var school = await FirstOrDefaultAsync(filters: new()
            {
                s => s.Id == id && s.Status == status
            }) ?? throw new EntityNotFoundException(MessageConstants.SchoolMessageConstrant.SchoolNotFound(id));
        return school;
    }
    public async Task<School?> GetSchoolByAreaIdAndAddress(Guid areaId, string address)
    {
        var school = await FirstOrDefaultAsync(filters: new()
            {
                s => s.AreaId == areaId,
                s => s.Address.ToLower() == address.ToLower()
            });
        return school;
    }
}