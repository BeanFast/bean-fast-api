using BusinessObjects.Models;
using DataTransferObjects.Models.Area.Request;
using DataTransferObjects.Models.Area.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums;

namespace Services.Interfaces
{
    public interface IAreaService
    {
        Task<Area> GetAreaByIdAsync(Guid id);
        Task<Area> GetAreaByIdAsync(int status, Guid id);
        Task<SearchAreaResponse> SearchAreaAsync(AreaFilterRequest request);
        Task<ICollection<string>> SearchCityNamesAsync(string cityName);
        Task<ICollection<string>> SearchDistrictNamesAsync(string cityName, string districtName);
        Task<ICollection<string>> SearchWardNamesAsync(string cityName, string districtName, string wardName);
    }
}
