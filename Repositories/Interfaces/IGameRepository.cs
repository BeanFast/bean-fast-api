using BusinessObjects.Models;
using DataTransferObjects.Models.Game.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IGameRepository : IGenericRepository<Game>
    {
        Task<Game> GetGameById(Guid id);
        Task<ICollection<GetGameResponse>> GetGamesAsync();
    }
}
