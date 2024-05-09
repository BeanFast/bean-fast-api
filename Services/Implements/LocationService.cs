using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
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
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Settings;
using Utilities.Statuses;
using Utilities.Utils;

namespace Services.Implements
{
    public class LocationService : BaseService<Location>, ILocationService
    {
        private readonly ICloudStorageService _cloudStorageService;
        //private readonly ISchoolService _schoolService;
        private readonly ILocationRepository _repository;
        public LocationService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings,
            ICloudStorageService cloudStorageService, ILocationRepository repository) : base(unitOfWork, mapper, appSettings)
        {
            _cloudStorageService = cloudStorageService;
            _repository = repository;
        }

        public async Task<ICollection<GetLocationResponse>> GetAllLocationAsync()
        {
            return await _repository.GetListAsync<GetLocationResponse>();
        }

        public async Task<Location> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<GetLocationResponse> GetLocationResponseByIdAsync(Guid id)
        {
            return _mapper.Map<GetLocationResponse>(await GetByIdAsync(id));
        }

        
        public async Task CreateLocationAsync(CreateLocationRequest request, User user)
        {
            var locationEntity = _mapper.Map<Location>(request);
            var locationId = Guid.NewGuid();
            locationEntity.Status = BaseEntityStatus.Active;
            //await _schoolService.GetSchoolByIdAsync(SchoolStatus.Active, request.SchoolId);
            var locationNumber = await _repository.CountAsync() + 1;
            locationEntity.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.LocationCodeConstrant.LocationPrefix, locationNumber);
            var duplicatedLocation = await GetLocationBySchoolIdAndNameAsync(request.SchoolId!.Value, request.Name);
            if (duplicatedLocation != null)
            {
                throw new InvalidRequestException(MessageConstants.LocationMessageConstrant.LocationAlreadyExists());
            }
            //Console.WriteLine();
            var imagePath = await _cloudStorageService.UploadFileAsync(
                locationId, _appSettings.Firebase.FolderNames.Location,
                request.Image);
            locationEntity.ImagePath = imagePath;
            locationEntity.Id = locationId;

            await _repository.InsertAsync(locationEntity, user);
            await _unitOfWork.CommitAsync();
        }


        public async Task UpdateLocationAsync(Guid id, UpdateLocationRequest request, User user)
        {
            var locationEntity = await GetByIdAsync(id);
            if (!request.Name!.Equals(locationEntity.Name) && !request.SchoolId.Equals(locationEntity.SchoolId))
            {
                //await _schoolService.GetSchoolByIdAsync(SchoolStatus.Active, request.SchoolId);
                var duplicatedLocation = await GetLocationBySchoolIdAndNameAsync(request.SchoolId, request.Name);
                if (duplicatedLocation != null)
                {
                    throw new InvalidRequestException(MessageConstants.LocationMessageConstrant.LocationAlreadyExists());
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

            await _repository.UpdateAsync(locationEntity, user);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteLocationAsync(Guid id, User user)
        {
            var locationEntity = await GetByIdAsync(id);
            await _repository.DeleteAsync(locationEntity, user);
            await _unitOfWork.CommitAsync();
        }
        public async Task<Location> GetLocationBySchoolIdAndNameAsync(Guid schoolId, string name)
        {
            return await _repository.GetLocationBySchoolIdAndNameAsync(schoolId, name);
        }

        public async Task<object> GetBestSellerLocationAsync(BestSellerLocationFilterRequest filterRequest)
        {
            return await _repository.GetBestSellerLocationAsync(filterRequest);
            //locations
        }
    }
}
