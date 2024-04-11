using BusinessObjects.Models;

namespace Services.Interfaces;

public interface IComboService : IBaseService
{
    Task CreateComboAsync(Combo combo, User user);
    Task CreateComboListAsync(List<Combo> combos, User user);

    Task DeleteComboAsync(Combo combo, User user);
    Task DeleteComboListAsync(ICollection<Combo> combos, User user);

    Task HardDeleteComboAsync(Combo combo);

    Task HardDeleteComboListAsync(ICollection<Combo> combos);

}