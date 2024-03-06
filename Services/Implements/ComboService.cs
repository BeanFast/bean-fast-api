using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Repositories.Interfaces;
using Services.Interfaces;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Statuses;
using static Utilities.Constants.EntityCodeConstrant;
using Utilities.Utils;
using Microsoft.Extensions.Options;
using Utilities.Settings;
using static Azure.Core.HttpHeader;

namespace Services.Implements;

public class ComboService : BaseService<Combo>, IComboService
{
  


    public ComboService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper, appSettings)
    {
        
    }

    public async Task CreateComboAsync(Combo combo)
    {
        combo.Status = ComboStatus.Active;
        combo.Id = Guid.NewGuid();
        combo.Code = EntityCodeUtil.GenerateUnnamedEntityCode(ComboCodeConstrant.ComboPrefix, combo.Id);
        await _repository.InsertAsync(combo);
    }
    public async Task CreateComboListAsync(List<Combo> combos)
    {
        foreach (var combo in combos)
        {
            await CreateComboAsync(combo);
        }
    }

    public async Task DeleteComboAsync(Combo combo)
    {
        await _repository.DeleteAsync(combo);
    }

    public async Task DeleteComboListAsync(ICollection<Combo> combos)
    {
        foreach (var combo in combos)
        {
            await DeleteComboAsync(combo);
        }
    }
    public async Task HardDeleteComboAsync(Combo combo)
    {
        await _repository.HardDeleteAsync(combo);
    }

    public async Task HardDeleteComboListAsync(ICollection<Combo> combos)
    {
        foreach (var combo in combos)
        {
            await HardDeleteComboAsync(combo);
        }
    }
}