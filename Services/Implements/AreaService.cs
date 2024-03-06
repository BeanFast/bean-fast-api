using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Area.Request;
using DataTransferObjects.Models.Area.Response;
using Diacritics.Extensions;
using Microsoft.EntityFrameworkCore;
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
using Utilities.Statuses;

namespace Services.Implements
{
    public class AreaService : BaseService<Area>, IAreaService
    {
        public AreaService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<Area> GetAreaByIdAsync(Guid id)
        {
            var area = await _repository.FirstOrDefaultAsync(filters: new()
            {
                area => area.Id == id
            }) ?? throw new EntityNotFoundException(MessageConstants.AreaMessageConstrant.AreaNotFound(id));
            return area;
        }

        public async Task<Area> GetAreaByIdAsync(int status, Guid id)
        {
            var area = await _repository.FirstOrDefaultAsync(filters: new()
            {
                area => area.Id == id,
                area => area.Status == status
            }) ?? throw new EntityNotFoundException(MessageConstants.AreaMessageConstrant.AreaNotFound(id));
            return area;
        }
        private ICollection<string> searchLocation(ICollection<string> locationNames, string searchQuery)
        {
            searchQuery = searchQuery.ToLower();
            return locationNames.Distinct().Where(name => searchQuery.HasDiacritics() ? name.ToLower().Contains(searchQuery) : name.ToLower().RemoveDiacritics().Contains(searchQuery)).ToList();
        }

        public async Task<ICollection<string>> SearchCityNamesAsync(string cityName)
        {
            cityName = cityName.ToLower();
            var cityNames = await _repository.GetListAsync(status: BaseEntityStatus.Active, filters: new()
            {
                area => area.Status == BaseEntityStatus.Active
            }, selector: c => c.City);
            return searchLocation(cityNames, cityName);
        }

        public async Task<ICollection<string>> SearchDistrictNamesAsync(string cityName, string districtName)
        {
            var districtNames = await _repository.GetListAsync(status: BaseEntityStatus.Active, filters: new()
            {
                area => area.City == cityName
            }, selector: d => d.District);
            return searchLocation(districtNames, districtName);
        }

        public async Task<ICollection<string>> SearchWardNamesAsync(string cityName, string districtName, string wardName)
        {
            var wardNames = await _repository.GetListAsync(status: BaseEntityStatus.Active, filters: new()
            {
                area => area.City == cityName,
                area => area.District == districtName,
            }, selector: d => d.Ward);
            return searchLocation(wardNames, wardName);
        }

        public async Task<SearchAreaResponse> SearchAreaAsync(AreaFilterRequest request)
        {
            return await _repository.FirstOrDefaultAsync(
                        status: BaseEntityStatus.Active,
                        filters: new()
                            {
                                area => area.City == request.City,
                                area => area.District == request.District,
                                area => area.Ward == request.Ward
                            },
                        include: i => i.Include(a => a.PrimarySchools),
                        selector: s => _mapper.Map<SearchAreaResponse>(s)
                        );
        }
    }
}