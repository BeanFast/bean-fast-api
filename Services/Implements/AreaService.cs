using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Area.Request;
using DataTransferObjects.Models.Area.Response;
using Diacritics.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Settings;
using Utilities.Statuses;

namespace Services.Implements
{
    public class AreaService : BaseService<Area>, IAreaService
    {
        private readonly IAreaRepository _areaRepository;
        public AreaService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, IAreaRepository areaRepository) : base(unitOfWork, mapper, appSettings)
        {
            _areaRepository = areaRepository;
        }

        public async Task<Area> GetAreaByIdAsync(Guid id)
        {
            return await _areaRepository.GetAreaByIdAsync(id);
        }
        public async Task<ICollection<SearchAreaResponse>> GetAllAsync()
        {
            return await _areaRepository.GetAllAsync();
        }

        public async Task<Area> GetAreaByIdAsync(int status, Guid id)
        {
            return await _areaRepository.GetAreaByIdAsync(status, id);
        }
       

        public async Task<ICollection<string>> SearchCityNamesAsync(string cityName)
        {
            return await _areaRepository.SearchCityNamesAsync(cityName);
        }

        public async Task<ICollection<string>> SearchDistrictNamesAsync(string cityName, string districtName)
        {
            return await _areaRepository.SearchDistrictNamesAsync(cityName, districtName);
        }

        public async Task<ICollection<string>> SearchWardNamesAsync(string cityName, string districtName, string wardName)
        {
            return await _areaRepository.SearchWardNamesAsync(cityName, districtName, wardName);
        }

        public async Task<SearchAreaResponse> SearchAreaAsync(AreaFilterRequest request)
        {
            return await _areaRepository.SearchAreaAsync(request);
        }
    }
}