﻿using BusinessObjects.Models;
using DataTransferObjects.Models.ExchangeGift.Request;
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
        }
    }
}
