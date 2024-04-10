using BusinessObjects.Models;
using DataTransferObjects.Models.Game.Request;
using DataTransferObjects.Models.Game.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public class GameMapper : AutoMapper.Profile
    {
        public GameMapper() 
        {
            CreateMap<Game, GetGameResponse>();
            CreateMap<CreateGameRequest, Game>();
        }
    }
}
