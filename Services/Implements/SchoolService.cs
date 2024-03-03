using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.School.Request;
using DataTransferObjects.Models.School.Response;
using Microsoft.Extensions.Options;
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
        private readonly AppSettings _appSettings;
        private readonly IAreaService _areaService;
        public SchoolService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, ICloudStorageService cloudStorageService, IOptions<AppSettings> appSettings, IAreaService areaService) : base(unitOfWork, mapper)
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
        public async Task<IPaginable<GetSchoolResponse>> GetSchoolPage(PaginationRequest paginationRequest, SchoolFilterRequest filterRequest)
        {
            var filters = GetSchoolFilterFromFilterRequest(filterRequest);
            Expression<Func<School, GetSchoolResponse>> selector = (s) => _mapper.Map<GetSchoolResponse>(s);
            var page = await _repository.GetPageAsync(
                    selector: selector,
                    filters: filters,
                    paginationRequest: paginationRequest
                );
            //var page = _repository.GetPageAsync(

            //    );
            return page;
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

        public async Task<School> GetByIdAsync(Guid id)
        {
            var school = await _repository.FirstOrDefaultAsync(filters: new()
            {
                s => s.Id == id
            }) ?? throw new EntityNotFoundException(MessageConstants.SchoolMessageConstrant.SchoolNotFound(id));
            return school;
        }
        public async Task<School> GetByIdAsync(int status, Guid id)
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
            schoolEntity.Code = EntityCodeUtil.GenerateNamedEntityCode(EntityCodeConstrant.SchoolCodeConstrant.SchoolPrefix, schoolEntity.Name, schoolId);
            await _areaService.GetAreaByIdAsync(AreaStatus.Active, request.AreaId);
            var dupplicatedSchool = await GetSchoolByAreaIdAndAddress(request.AreaId, request.Address);
            if(dupplicatedSchool != null)
            {
                throw new DataExistedException(MessageConstants.SchoolMessageConstrant.SchoolAlreadyExists());
            }
            var imagePath = await _cloudStorageService.UploadFileAsync(
                schoolId, _appSettings.Firebase.FolderNames.School, 
                request.Image.ContentType, request.Image);
            schoolEntity.ImagePath = imagePath;
            schoolEntity.Id = schoolId;

            await _repository.InsertAsync(schoolEntity);

        }

        public async Task DeleteSchoolAsync(Guid id)
        {
            var schoolEntity = await GetByIdAsync(SchoolStatus.Active, id);
            await _repository.DeleteAsync(schoolEntity);
        }
    }
}
