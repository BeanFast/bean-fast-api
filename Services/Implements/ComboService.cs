using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Repositories.Interfaces;
using Services.Interfaces;
using Utilities.Enums;
using Utilities.Statuses;

namespace Services.Implements;

public class ComboService : BaseService<Combo>, IComboService
{
    public async Task CreateCombo(Combo combo)
    {
        combo.Status = BaseEntityStatus.Active;
        combo.Code = combo.Id.ToString();
        
    }

    public ComboService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
        
    }
}