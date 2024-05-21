using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Wallet.Response;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Statuses;

namespace Repositories.Implements
{
    public class WalletRepository : GenericRepository<Wallet>, IWalletRepository
    {
        public WalletRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public async Task<GetWalletTypeMoneyByCustomerId> GetWalletTypeMoneyByCustomerIdAsync(Guid customerId)
        {
            List<Expression<Func<Wallet, bool>>> filters = new()
            {
                p => p.UserId == customerId && WalletType.Money.ToString().Equals(p.Type)
            };
            var wallet = await FirstOrDefaultAsync<GetWalletTypeMoneyByCustomerId>(filters: filters);
            return wallet;
        }
        public async Task<ICollection<GetWalletByCurrentCustomerAndProfileResponse>> GetWalletByCurrentCustomerAndProfileAsync(Guid customerId)
        {
            List<Expression<Func<Wallet, bool>>> filters = new()
            {
                p => p.UserId == customerId && p.Status != BaseEntityStatus.Deleted
            };
            //if (profileId != null) filters.Add(p => p.ProfileId == profileId);
            var wallets = await GetListAsync<GetWalletByCurrentCustomerAndProfileResponse>(
                filters: filters);
            return wallets;
        }
        public async Task<Wallet> GetPointWalletByUserIdAndProfildId(Guid userId)
        {
            List<Expression<Func<Wallet, bool>>> filters = new()
            {
                p => p.UserId == userId && WalletType.Points.ToString().Equals(p.Type) //&& p.ProfileId == profileId
            };
            var wallet = await FirstOrDefaultAsync(filters: filters);
            return wallet!;
        }
        public Task<Wallet> GetByIdAsync(Guid walletId)
        {
            var filters = new List<Expression<Func<Wallet, bool>>>
            {
                p => p.Id == walletId && p.Status != BaseEntityStatus.Deleted
            };
            var result = FirstOrDefaultAsync(filters)
                ?? throw new EntityNotFoundException(MessageConstants.WalletMessageConstrant.WalletNotFound(walletId));
            return result!;
        }
        public async Task<Wallet> GetMoneyWalletByUserId(Guid userId)
        {
            var filters = new List<Expression<Func<Wallet, bool>>>
            {
                p => p.UserId == userId && p.Type == WalletType.Money.ToString()// && p.ProfileId == null
            };
            var result = await FirstOrDefaultAsync(filters);
            return result!;
        }
    }
}
