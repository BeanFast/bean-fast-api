using BusinessObjects.Models;

namespace Services.Interfaces;

public interface IComboService
{
    Task CreateCombo(Combo combo);
}