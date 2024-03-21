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
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Exceptions;
using Utilities.Settings;
using Utilities.Statuses;
using Utilities.Utils;

namespace Services.Implements
{
    public class LocationService : BaseService<Location>, ILocationService
    {
        private readonly ICloudStorageService _cloudStorageService;
        private readonly ISchoolService _schoolService;
        public LocationService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings,
            ICloudStorageService cloudStorageService, ISchoolService schoolService) : base(unitOfWork, mapper, appSettings)
        {
            _cloudStorageService = cloudStorageService;
            _schoolService = schoolService;
        }

        public async Task<ICollection<GetLocationResponse>> GetAllLocationAsync()
        {
            Expression<Func<Location, GetLocationResponse>> selector = (l => _mapper.Map<GetLocationResponse>(l));
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

        public async Task<Location> GetLocationBySchoolIdAndNameAsync(Guid schoolId, string name)
        {
            var location = await _repository.FirstOrDefaultAsync(filters: new()
            {
                l => l.SchoolId == schoolId,
                s => s.Name.ToLower() == name.ToLower()
            });
            return location!;
        }

        public async Task CreateLocationAsync(CreateLocationRequest request)
        {
            var locationEntity = _mapper.Map<Location>(request);
            var locationId = Guid.NewGuid();
            locationEntity.Code = EntityCodeUtil.GenerateNamedEntityCode(EntityCodeConstrant.LocationCodeConstrant.LocationPrefix, locationEntity.Name, locationId);
            locationEntity.Status = BaseEntityStatus.Active;
            await _schoolService.GetSchoolByIdAsync(SchoolStatus.Active, request.SchoolId);
            var duplicatedLocation = await GetLocationBySchoolIdAndNameAsync(request.SchoolId, request.Name);
            if (duplicatedLocation != null)
            {
                throw new DataExistedException(MessageConstants.LocationMessageConstrant.LocationAlreadyExists());
            }
            var imagePath = await _cloudStorageService.UploadFileAsync(
                locationId, _appSettings.Firebase.FolderNames.Location,
                request.Image);
            locationEntity.ImagePath = imagePath;
            locationEntity.Id = locationId;

            await _repository.InsertAsync(locationEntity);
            await _unitOfWork.CommitAsync();
        }


        public async Task UpdateLocationAsync(Guid id, UpdateLocationRequest request)
        {
            var locationEntity = await GetByIdAsync(id);
            if (!request.Name!.Equals(locationEntity.Name) && !request.SchoolId.Equals(locationEntity.SchoolId))
            {
                await _schoolService.GetSchoolByIdAsync(SchoolStatus.Active, request.SchoolId);
                var duplicatedLocation = await GetLocationBySchoolIdAndNameAsync(request.SchoolId, request.Name);
                if (duplicatedLocation != null)
                {
                    throw new DataExistedException(MessageConstants.LocationMessageConstrant.LocationAlreadyExists());
                }
                locationEntity.SchoolId = request.SchoolId;
                locationEntity.Name = request.Name;
            }
            locationEntity.Description = request.Description!;
            locationEntity.Status = request.Status;
            if (request.Image != null)
            {
                await _cloudStorageService.DeleteFileAsync(locationEntity.Id, _appSettings.Firebase.FolderNames.Location);
                var imagePath = await _cloudStorageService.UploadFileAsync(
                locationEntity.Id, _appSettings.Firebase.FolderNames.Location,
                request.Image);
                locationEntity.ImagePath = imagePath;
            }

            await _repository.UpdateAsync(locationEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteLocationAsync(Guid id)
        {
            var locationEntity = await GetByIdAsync(id);
            await _repository.DeleteAsync(locationEntity);
            await _unitOfWork.CommitAsync();
        }
    }
}
