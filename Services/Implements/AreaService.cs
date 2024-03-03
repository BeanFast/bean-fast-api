using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
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
    }
}
