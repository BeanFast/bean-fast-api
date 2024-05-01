using BusinessObjects.Models;
using DataTransferObjects.Models.ExchangeGift.Request;
using DataTransferObjects.Models.ExchangeGift.Response;
using DataTransferObjects.Models.Order.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public class ExchangeGiftMapper : AutoMapper.Profile
    {
        public ExchangeGiftMapper()
        {
            CreateMap<CreateExchangeGiftRequest, ExchangeGift>();

            CreateMap<ExchangeGift, GetExchangeGiftResponse>();
            CreateMap<Gift, GetExchangeGiftResponse.GiftOfGetExchangeGiftResponse>();
            CreateMap<SessionDetail, GetExchangeGiftResponse.SessionDetailOfExchangeGiftResponse>();
            CreateMap<Profile, GetExchangeGiftResponse.ProfileOfExchangeGiftResponse>();
            CreateMap<Session, GetExchangeGiftResponse.SessionDetailOfExchangeGiftResponse.GetSessionOfSessionDetail>();
            CreateMap<Location, GetExchangeGiftResponse.SessionDetailOfExchangeGiftResponse.LocationOfSessionDetail>();
            CreateMap<School, GetExchangeGiftResponse.SessionDetailOfExchangeGiftResponse.LocationOfSessionDetail.SchoolOfLocation>();
            CreateMap<Area, GetExchangeGiftResponse.SessionDetailOfExchangeGiftResponse.LocationOfSessionDetail.SchoolOfLocation.AreaOfLocation>();
        }
    }
}
