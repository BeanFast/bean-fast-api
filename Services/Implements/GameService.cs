using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Game.Request;
using DataTransferObjects.Models.Game.Response;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Exceptions;
using Utilities.Settings;
using Utilities.Statuses;
using Utilities.Utils;

namespace Services.Implements
{
    public class GameService : BaseService<Game>, IGameService
    {
        private readonly IGameRepository _repository;
        public GameService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, IGameRepository repository) : base(unitOfWork, mapper, appSettings)
        {
            _repository = repository;
        }

        public async Task CreateGameAsync(CreateGameRequest request, User user)
        {
            var gameEntity = _mapper.Map<Game>(request);
            gameEntity.Id = Guid.NewGuid();
            gameEntity.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.GameCodeConstraint.GamePrefix, await _repository
                .CountAsync() + 1);
            gameEntity.Status = BaseEntityStatus.Active;
            await _repository.InsertAsync(gameEntity, user);
            await _unitOfWork.CommitAsync();
        }

        public async Task<Game> GetGameById(Guid id)
        {
            return await _repository.GetGameById(id);
        }

        public async Task<ICollection<GetGameResponse>> GetGamesAsync()
        {
            return await _repository.GetGamesAsync();
        }
    }
}
