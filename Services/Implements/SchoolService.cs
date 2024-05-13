using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.School.Request;
using DataTransferObjects.Models.School.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Linq.Expressions;
using Twilio.Rest.Trunking.V1;
using Utilities.Constants;
using Utilities.Exceptions;
using Utilities.Settings;
using Utilities.Statuses;
using Utilities.Utils;

namespace Services.Implements
{
    public class SchoolService : BaseService<School>, ISchoolService
    {
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IAreaService _areaService;
        private readonly ILocationService _locationService;
        private readonly ISchoolRepository _repository;
        //private readonly I
        public SchoolService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, ICloudStorageService cloudStorageService, IOptions<AppSettings> appSettings, IAreaService areaService, ILocationService locationService, ISchoolRepository repository) : base(unitOfWork, mapper, appSettings)
        {
            _cloudStorageService = cloudStorageService;
            _appSettings = appSettings.Value;
            _areaService = areaService;
            _locationService = locationService;
            _repository = repository;
        }
        
        public async Task<IPaginable<GetSchoolIncludeAreaAndLocationResponse>> GetSchoolPageAsync(PaginationRequest paginationRequest, SchoolFilterRequest filterRequest)
        {
            return await _repository.GetSchoolPageAsync(paginationRequest, filterRequest);
        }
        public async Task<ICollection<GetSchoolIncludeAreaAndLocationResponse>> GetSchoolListAsync(PaginationRequest paginationRequest, SchoolFilterRequest filterRequest)
        {
            return await _repository.GetSchoolListAsync(paginationRequest, filterRequest);
        }

        public async Task<School?> GetSchoolByAreaIdAndAddress(Guid areaId, string address)
        {
            return await _repository.GetSchoolByAreaIdAndAddress(areaId, address);
        }


        public async Task<School> GetSchoolByIdAsync(Guid id)
        {
            return await _repository.GetSchoolByIdAsync(id);
        }
        public async Task<School> GetSchoolByIdAsync(int status, Guid id)
        {
            return await _repository.GetSchoolByIdAsync(status, id);
        }

        public async Task CreateSchoolAsync(CreateSchoolRequest request, User user)
        {

            var schoolEntity = _mapper.Map<School>(request);
            var schoolId = Guid.NewGuid();
            var schoolNumber = await _repository.CountAsync() + 1;
            schoolEntity.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.SchoolCodeConstrant.SchoolPrefix, schoolNumber);
            await _areaService.GetAreaByIdAsync(AreaStatus.Active, request.AreaId);
            var duplicatedSchool = await GetSchoolByAreaIdAndAddress(request.AreaId, request.Address);
            if (duplicatedSchool != null)
            {
                throw new InvalidRequestException(MessageConstants.SchoolMessageConstrant.SchoolAlreadyExists());
            }
            var imagePath = await _cloudStorageService.UploadFileAsync(
                schoolId, _appSettings.Firebase.FolderNames.School,
                request.Image);
            schoolEntity.ImagePath = imagePath;
            schoolEntity.Id = schoolId;
            schoolEntity.Locations = null;
            await _repository.InsertAsync(schoolEntity, user);
            await _unitOfWork.CommitAsync();
            foreach (var location in request.Locations)
            {
                location.SchoolId = schoolId;
                await _locationService.CreateLocationAsync(location, user);
            }
        }

        public async Task DeleteSchoolAsync(Guid id, User user)
        {
            var schoolEntity = await GetSchoolByIdAsync(SchoolStatus.Active, id);
            await _repository.DeleteAsync(schoolEntity, user);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateSchoolAsync(Guid id, UpdateSchoolRequest request, User user)
        {
            var schoolEntity = await GetSchoolByIdAsync(id);
            if (!request.Address.Equals(schoolEntity.Address) && !request.AreaId.Equals(request.AreaId))
            {
                await _areaService.GetAreaByIdAsync(AreaStatus.Active, id);
                var dupplicatedSchool = await GetSchoolByAreaIdAndAddress(request.AreaId, request.Address);
                if (dupplicatedSchool != null)
                {
                    throw new InvalidRequestException(MessageConstants.SchoolMessageConstrant.SchoolAlreadyExists());
                }
                schoolEntity.AreaId = request.AreaId;
                schoolEntity.Address = request.Address;
            }
            schoolEntity.Name = request.Name;
            if (request.Image != null)
            {
                await _cloudStorageService.DeleteFileAsync(schoolEntity.Id, _appSettings.Firebase.FolderNames.School);
                var imagePath = await _cloudStorageService.UploadFileAsync(
                schoolEntity.Id, _appSettings.Firebase.FolderNames.School,
                request.Image);
                schoolEntity.ImagePath = imagePath;
            }


            await _repository.UpdateAsync(schoolEntity, user);
            await _unitOfWork.CommitAsync();
        }

        public async Task<School> GetByIdIncludeProfile(Guid schoolId)
        {
            return await _repository.GetByIdIncludeProfile(schoolId);
        }
        public async Task<int> CountStudentAsync(Guid schoolId)
        {
            var school = await GetByIdIncludeProfile(schoolId);
            return school.Profiles!.Count();
        }

        public async Task<GetSchoolIncludeAreaAndLocationResponse> GetSchoolIncludeAreaAndLocationResponseByIdAsync(Guid id)
        {
            var school = await _repository.FirstOrDefaultAsync<GetSchoolIncludeAreaAndLocationResponse>(
                filters: new()
                    {
                        s => s.Id == id && s.Status == BaseEntityStatus.Active
                    }, 
                include: i => i.Include(s => s.Area).Include(s => s.Locations!)) ?? throw new EntityNotFoundException(MessageConstants.SchoolMessageConstrant.SchoolNotFound(id));
            return school!;
        }
    }
}
