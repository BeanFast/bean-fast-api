using BusinessObjects.Models;

namespace Services.Interfaces;

public interface IComboService : IBaseService
{
    Task CreateComboAsync(Combo combo);
    Task CreateComboListAsync(List<Combo> combos);

    Task DeleteComboAsync(Combo combo);
    Task DeleteComboListAsync(ICollection<Combo> combos);

    Task HardDeleteComboAsync(Combo combo);

    Task HardDeleteComboListAsync(ICollection<Combo> combos);

}