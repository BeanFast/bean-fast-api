using BusinessObjects.Models;
using DataTransferObjects.Models.CardType.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICardTypeService : IBaseService
    {
        Task CreateCardTypeAsync(CreateCardTypeRequest request, User creator);
        Task<CardType> GetByIdAsync(Guid id);
        Task<ICollection<GetCardTypeResponse>> GetAllAsync();
        Task UpdateCardTypeAsync(Guid id, UpdateCardTypeRequest request, User updater);
        Task DeleteCardTypeAsync(Guid id, User deleter);
    }
}
