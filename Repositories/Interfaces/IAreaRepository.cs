using BusinessObjects.Models;
using DataTransferObjects.Models.Area.Request;
using DataTransferObjects.Models.Area.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IAreaRepository : IGenericRepository<Area>
    {
        Task<Area> GetAreaByIdAsync(Guid id);
        Task<SearchAreaResponse> SearchAreaAsync(AreaFilterRequest request);
        Task<ICollection<string>> SearchDistrictNamesAsync(string cityName, string districtName);
        Task<ICollection<string>> SearchCityNamesAsync(string cityName);
        Task<Area> GetAreaByIdAsync(int status, Guid id);
        Task<ICollection<SearchAreaResponse>> GetAllAsync();
        Task<ICollection<string>> SearchWardNamesAsync(string cityName, string districtName, string wardName);
    }

}
