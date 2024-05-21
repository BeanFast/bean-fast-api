using BusinessObjects.Models;
using DataTransferObjects.Models.Wallet.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IWalletRepository : IGenericRepository<Wallet>
    {
        Task<Wallet> GetPointWalletByUserIdAndProfildId(Guid userId);
        Task<ICollection<GetWalletByCurrentCustomerAndProfileResponse>> GetWalletByCurrentCustomerAndProfileAsync(Guid customerId);
        Task<GetWalletTypeMoneyByCustomerId> GetWalletTypeMoneyByCustomerIdAsync(Guid customerId);
        Task<Wallet> GetByIdAsync(Guid walletId);
        Task<Wallet> GetMoneyWalletByUserId(Guid userId);
    }
}
