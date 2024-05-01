using BusinessObjects.Models;
using DataTransferObjects.Models.Wallet.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums;

namespace Services.Interfaces
{
    public interface IWalletService : IBaseService
    {
        Task<ICollection<GetWalletByCurrentCustomerAndProfileResponse>> GetWalletByCurrentCustomerAndProfileAsync(Guid customerId, Guid? profileId);
        Task<GetWalletTypeMoneyByCustomerId> GetWalletTypeMoneyByCustomerIdAsync(Guid customerId);
        Task<Wallet> GetMoneyWalletByUserId(Guid userId);
        Task<Wallet> GetByIdAsync(Guid customerId);
        Task CreateWalletAsync(WalletType type, Wallet wallet);
        Task UpdateAsync(Wallet wallet);
    }
}
