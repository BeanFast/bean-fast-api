using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Location.Request;
using DataTransferObjects.Models.Location.Response;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Exceptions;
using Utilities.Statuses;

namespace Repositories.Implements
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        public LocationRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public async Task<Location> GetByIdAsync(Guid id)
        {
            List<Expression<Func<Location, bool>>> filters = new()
            {
                (location) => location.Id == id
            };
            var location = await FirstOrDefaultAsync(filters: filters)
                ?? throw new EntityNotFoundException(MessageConstants.LocationMessageConstrant.LocationlNotFound(id));
            return location;
        }
        public async Task<Location> GetLocationBySchoolIdAndNameAsync(Guid schoolId, string name)
        {
            var location = await FirstOrDefaultAsync(filters: new()
            {
                l => l.SchoolId == schoolId,
                l => l.Name.ToLower() == name.ToLower()
            });
            return location!;
        }

        public async Task<object> GetBestSellerLocationAsync(BestSellerLocationFilterRequest filterRequest)
        {
            var filters = new List<Expression<Func<Location, bool>>>();
            Func<IQueryable<Location>, IIncludableQueryable<Location, object>> include;

            if (filterRequest.StartDate != DateTime.MinValue && filterRequest.EndDate != DateTime.MinValue)
            {
                include = (i) => i.Include(f => f.SessionDetails!).ThenInclude(sd => sd.Orders!.Where(o => o.PaymentDate >= filterRequest.StartDate && o.PaymentDate <= filterRequest.EndDate));
            }
            else
            {
                include = i => i.Include(l => l.SessionDetails!);
            }
            if (filterRequest.SchoolId != null && filterRequest.SchoolId != Guid.Empty)
            {
                filters.Add(l => l.SchoolId == filterRequest.SchoolId);
            }
            filters.Add(l => l.Status == BaseEntityStatus.Active);
            var locations = await GetListAsync(
                filters: filters,
                include: include);
            var data = locations.OrderByDescending(l =>
            {
                return l.SessionDetails!.Sum(sd => sd.Orders!.Where(o => o.Status == OrderStatus.Completed).Count());
            }).ToList();
            return _mapper.Map<ICollection<GetBestSellerLocationResponse>>(data);
            //locations
        }
    }
}
