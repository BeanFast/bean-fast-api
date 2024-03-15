using BusinessObjects.Models;
using DataTransferObjects.Models.Wallet.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public class WalletMapper : AutoMapper.Profile
    {
        public WalletMapper()
        {
            CreateMap<Wallet, GetWalletByCurrentCustomerAndProfileResponse>();
        }
    }
}
