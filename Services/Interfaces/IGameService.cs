using BusinessObjects.Models;
using DataTransferObjects.Models.Game.Request;
using DataTransferObjects.Models.Game.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IGameService
    {
        Task<ICollection<GetGameResponse>> GetGamesAsync();
        Task<Game> GetGameById(Guid id);
        Task CreateGameAsync(CreateGameRequest request, User user);
    }
}
