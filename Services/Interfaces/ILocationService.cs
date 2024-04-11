using BusinessObjects.Models;
using DataTransferObjects.Models.Location.Request;
using DataTransferObjects.Models.Location.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ILocationService : IBaseService
    {
        Task CreateLocationAsync(CreateLocationRequest request, User user);
        Task<ICollection<GetLocationResponse>> GetAllLocationAsync();
        Task<Location> GetLocationBySchoolIdAndNameAsync(Guid schoolId, string name);
        Task<Location> GetByIdAsync(Guid id);
        Task DeleteLocationAsync(Guid id, User user);
        Task UpdateLocationAsync(Guid id, UpdateLocationRequest request, User user);
    }
}
