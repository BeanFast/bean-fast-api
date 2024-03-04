using DataTransferObjects.Models.Location.Request;
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
        Task DeleteLocationAsync(Guid id);
        Task UpdateLocationAsync(Guid id, UpdateLocationRequest request);
    }
}
