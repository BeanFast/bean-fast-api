using BusinessObjects.Models;
using DataTransferObjects.Models.Location.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ILocationRepository : IGenericRepository<Location>
    {
        Task<Location> GetByIdAsync(Guid id);
        Task<Location> GetLocationBySchoolIdAndNameAsync(Guid schoolId, string name);
        Task<object> GetBestSellerLocationAsync(BestSellerLocationFilterRequest filterRequest);
    }
}
