using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Location.Request;
using DataTransferObjects.Models.Location.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Exceptions;
using Utilities.Settings;
using Utilities.Statuses;

namespace Services.Implements
{
    public class LocationService : BaseService<Location>, ILocationService
    {
        public LocationService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper, appSettings)
        {
        }

        public async Task<ICollection<GetLocationResponse>> GetAllLocationAsync()
        {
            Expression<Func<Location, GetLocationResponse>> selector = (l => _mapper.Map<GetLocationResponse>(l));
            //Func<IQueryable<Location>, IIncludableQueryable<Location, object>> include = (l) => l.Include(l => l.School!);
            return await _repository.GetListAsync(BaseEntityStatus.Active, selector: selector);
        }

        public async Task<Location> GetByIdAsync(Guid id)
        {
            List<Expression<Func<Location, bool>>> filters = new()
            {
                (location) => location.Id == id
            };
            var location = await _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active, filters: filters)
                ?? throw new EntityNotFoundException(MessageConstants.LocationMessageConstrant.LocationlNotFound(id));
            return location;
        }

        public async Task<GetLocationResponse> GetLocationResponseByIdAsync(Guid id)
        {
            return _mapper.Map<GetLocationResponse>(await GetByIdAsync(id));
        }

        public async Task CreateLocationAsync(CreateLocationRequest request)
        {
            throw new NotImplementedException();
        }


        public async Task UpdateLocationAsync(Guid id, UpdateLocationRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteLocationAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
