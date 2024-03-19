using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.School.Request;
using DataTransferObjects.Models.School.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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
        //private readonly I
        public SchoolService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, ICloudStorageService cloudStorageService, IOptions<AppSettings> appSettings, IAreaService areaService) : base(unitOfWork, mapper, appSettings)
        {
            _cloudStorageService = cloudStorageService;
            _appSettings = appSettings.Value;
            _areaService = areaService;
        }
        private List<Expression<Func<School, bool>>> GetSchoolFilterFromFilterRequest(SchoolFilterRequest filterRequest)
        {
            List<Expression<Func<School, bool>>> filters = new();
            if (filterRequest.Code != null)
            {
                filters.Add(s => s.Code == filterRequest.Code);
            }
            if (filterRequest.Name != null)
            {
                filters.Add(s => s.Name.ToLower().Contains(filterRequest.Name.ToLower()));
            }
            if (filterRequest.Address != null)
            {
                filters.Add(s => s.Address.ToLower().Contains(filterRequest.Address.ToLower()));
            }

            return filters;
        }
        public async Task<IPaginable<GetSchoolIncludeAreaAndLocationResponse>> GetSchoolPageAsync(PaginationRequest paginationRequest, SchoolFilterRequest filterRequest)
        {
            var filters = GetSchoolFilterFromFilterRequest(filterRequest);
            var page = await _repository.GetPageAsync<GetSchoolIncludeAreaAndLocationResponse>(

                    filters: filters,
                    paginationRequest: paginationRequest,
                    include: s => s.Include(s => s.Area).Include(s => s.Locations!)
                );
            //var page = _repository.GetPageAsync(

            //    );
            return page;
        }
        public async Task<ICollection<GetSchoolIncludeAreaAndLocationResponse>> GetSchoolListAsync(PaginationRequest paginationRequest, SchoolFilterRequest filterRequest)
        {
            var filters = GetSchoolFilterFromFilterRequest(filterRequest);
            return await _repository.GetListAsync<GetSchoolIncludeAreaAndLocationResponse>(
                filters: filters,
                include: s => s.Include(s => s.Area).Include(s => s.Locations!.Where(l => l.Status == BaseEntityStatus.Active))
            );
        }

        public async Task<School?> GetSchoolByAreaIdAndAddress(Guid areaId, string address)
        {
            var school = await _repository.FirstOrDefaultAsync(filters: new()
            {
                s => s.AreaId == areaId,
                s => s.Address.ToLower() == address.ToLower()
            });
            return school;
        }


        public async Task<School> GetSchoolByIdAsync(Guid id)
        {
            var school = await _repository.FirstOrDefaultAsync(filters: new()
            {
                s => s.Id == id
            }) ?? throw new EntityNotFoundException(MessageConstants.SchoolMessageConstrant.SchoolNotFound(id));
            return school;
        }
        public async Task<School> GetSchoolByIdAsync(int status, Guid id)
        {
            var school = await _repository.FirstOrDefaultAsync(status, filters: new()
            {
                s => s.Id == id
            }) ?? throw new EntityNotFoundException(MessageConstants.SchoolMessageConstrant.SchoolNotFound(id));
            return school;
        }

        public async Task CreateSchoolAsync(CreateSchoolRequest request)
        {
            var schoolEntity = _mapper.Map<School>(request);
            var schoolId = Guid.NewGuid();
            var schoolNumber = await _repository.CountAsync() + 1;
            schoolEntity.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.SchoolCodeConstrant.SchoolPrefix, schoolNumber);
            await _areaService.GetAreaByIdAsync(AreaStatus.Active, request.AreaId);
            var duplicatedSchool = await GetSchoolByAreaIdAndAddress(request.AreaId, request.Address);
            if (duplicatedSchool != null)
            {
                throw new DataExistedException(MessageConstants.SchoolMessageConstrant.SchoolAlreadyExists());
            }
            var imagePath = await _cloudStorageService.UploadFileAsync(
                schoolId, _appSettings.Firebase.FolderNames.School,
                request.Image);
            schoolEntity.ImagePath = imagePath;
            schoolEntity.Id = schoolId;

            await _repository.InsertAsync(schoolEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteSchoolAsync(Guid id)
        {
            var schoolEntity = await GetSchoolByIdAsync(SchoolStatus.Active, id);
            await _repository.DeleteAsync(schoolEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateSchoolAsync(Guid id, UpdateSchoolRequest request)
        {
            var schoolEntity = await GetSchoolByIdAsync(id);
            if (!request.Address.Equals(schoolEntity.Address) && !request.AreaId.Equals(request.AreaId))
            {
                await _areaService.GetAreaByIdAsync(AreaStatus.Active, id);
                var dupplicatedSchool = await GetSchoolByAreaIdAndAddress(request.AreaId, request.Address);
                if (dupplicatedSchool != null)
                {
                    throw new DataExistedException(MessageConstants.SchoolMessageConstrant.SchoolAlreadyExists());
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


            await _repository.UpdateAsync(schoolEntity);
            await _unitOfWork.CommitAsync();
        }

    }
}
