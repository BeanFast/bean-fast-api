using BusinessObjects.Models;
using DataTransferObjects.Models.CardType.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public class CardTypeMapper : AutoMapper.Profile
    {
        public CardTypeMapper()
        {
            CreateMap<CardType, GetCardTypeResponse>();
            CreateMap<CreateCardTypeRequest, CardType>();
        }
    }
}
