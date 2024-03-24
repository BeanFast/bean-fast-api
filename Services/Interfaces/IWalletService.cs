using DataTransferObjects.Models.Wallet.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IWalletService : IBaseService
    {
        Task<ICollection<GetWalletByCurrentCustomerAndProfileResponse>> GetWalletByCurrentCustomerAndProfileAsync(Guid customerId, Guid? profileId);
    }
}
