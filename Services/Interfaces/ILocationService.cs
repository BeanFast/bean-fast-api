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
    public interface ILocationService
    {
        Task CreateLocationAsync(CreateLocationRequest request);
        Task<ICollection<GetLocationResponse>> GetAllLocationAsync();
        Task<GetLocationResponse> GetLocationResponseByIdAsync(Guid id);
        Task<Location> GetByIdAsync(Guid id);
        Task DeleteLocationAsync(Guid id);
        Task UpdateLocationAsync(Guid id, UpdateLocationRequest request);
    }
}
