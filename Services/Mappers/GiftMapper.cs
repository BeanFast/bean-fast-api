using BusinessObjects.Models;
using DataTransferObjects.Models.Gift.Request;
using DataTransferObjects.Models.Gift.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public class GiftMapper : AutoMapper.Profile
    {
        public GiftMapper()
        {
            CreateMap<Gift, GetGiftResponse>();
            CreateMap<CreateGiftRequest, Gift>();
        }
    }
}
