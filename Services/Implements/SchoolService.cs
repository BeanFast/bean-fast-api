using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.School.Request;
using DataTransferObjects.Models.School.Response;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implements
{
    public class SchoolService : BaseService<School>, ISchoolService
    {
        public SchoolService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }
        private List<Expression<Func<School, bool>>> GetSchoolFilterFromFilterRequest(SchoolFilterRequest filterRequest)
        {
            List<Expression<Func<School, bool>>> filters = new();
            if (filterRequest.Code != null)
            {
                filters.Add(s => s.Code ==  filterRequest.Code);
            }
            if (filterRequest.Name != null)
            {
                filters.Add(s => s.Name.ToLower().Contains(filterRequest.Name.ToLower()));
            }
            if(filterRequest.Address != null)
            {
                filters.Add(s => s.Address.ToLower().Contains(filterRequest.Address.ToLower()));
            }
            return filters;
        }
        public async Task<IPaginable<GetSchoolResponse>> GetSchoolPage(PaginationRequest paginationRequest, SchoolFilterRequest filterRequest)
        {
            var filters = GetSchoolFilterFromFilterRequest(filterRequest);
            Expression<Func<School, GetSchoolResponse>> selector = (s) => _mapper.Map<GetSchoolResponse>(s);
            var page = await _repository.GetPageAsync(
                    selector: selector, 
                    filters: filters,
                    paginationRequest: paginationRequest
                );
            //var page = _repository.GetPageAsync(

            //    );
            return page;
        }
    }
}
