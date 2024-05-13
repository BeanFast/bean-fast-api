using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Game.Response;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Exceptions;
using Utilities.Statuses;

namespace Repositories.Implements
{
    public class GameRepository : GenericRepository<Game>, IGameRepository
    {
        public GameRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public async Task<Game> GetGameById(Guid id)
        {
            var filters = new List<Expression<Func<Game, bool>>>
            {
                game => game.Id == id,
                game => game.Status == BaseEntityStatus.Active
            };
            var result = await FirstOrDefaultAsync(filters);
            if (result == null) throw new EntityNotFoundException(MessageConstants.GameMessageConstrant.GameNotFound(id));
            return result;
        }
        public async Task<ICollection<GetGameResponse>> GetGamesAsync()
        {
            var filters = new List<Expression<Func<Game, bool>>>
            {
                g => g.Status != BaseEntityStatus.Deleted
            };
            var result = await GetListAsync<GetGameResponse>(
                    filters: filters
                );
            return result;
        }
    }
}
