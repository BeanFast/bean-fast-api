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

    public async Task CreateComboAsync(Combo combo, User user)
    {
        combo.Status = ComboStatus.Active;
        combo.Id = Guid.NewGuid();
        var comboNumber = await _repository.CountAsync() + 1;
        combo.Code = EntityCodeUtil.GenerateEntityCode(ComboCodeConstrant.ComboPrefix, comboNumber);
        await _repository.InsertAsync(combo, user);
        await _unitOfWork.CommitAsync();
    }
    public async Task CreateComboListAsync(List<Combo> combos, User user)
    {
        foreach (var combo in combos)
        {
            await CreateComboAsync(combo, user);
        }
        //await _unitOfWork.CommitAsync();
    }

    public async Task DeleteComboAsync(Combo combo, User user)
    {
        await _repository.DeleteAsync(combo, user);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteComboListAsync(ICollection<Combo> combos, User user)
    {
        foreach (var combo in combos)
        {
            await DeleteComboAsync(combo,user);
        }
        //await _unitOfWork.CommitAsync();
    }
    public async Task HardDeleteComboAsync(Combo combo)
    {
        await _repository.HardDeleteAsync(combo);
        await _unitOfWork.CommitAsync();
    }

    public async Task HardDeleteComboListAsync(ICollection<Combo> combos)
    {
        foreach (var combo in combos)
        {
            await HardDeleteComboAsync(combo);
        }
        //await _unitOfWork.CommitAsync();
    }
}