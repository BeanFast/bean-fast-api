using DataTransferObjects.Models.CardType.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICardTypeService
    {
        Task CreateCardTypeAsync(CreateCardTypeRequest request);
        Task<ICollection<GetCardTypeResponse>> GetAllAsync();
        Task UpdateCardTypeAsync(Guid id, UpdateCardTypeRequest request);
        Task DeleteCardTypeAsync(Guid id);
    }
}
