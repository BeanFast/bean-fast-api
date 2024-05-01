using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Wallet.Response;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Settings;
using Utilities.Statuses;
using Utilities.Utils;

namespace Services.Implements
{
    public class WalletService : BaseService<Wallet>, IWalletService
    {
        public WalletService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper, appSettings)
        {
        }

        public async Task CreateWalletAsync(WalletType type, Wallet wallet)
        {
            wallet.Balance = 0;
            wallet.Status = BaseEntityStatus.Active;
            string walletPrefix = "";
            if (type.Equals(WalletType.Money))
            {
                wallet.Type = WalletType.Money.ToString();
                wallet.Name = "Ví tiền của " + wallet.Name;
                walletPrefix = EntityCodeConstrant.WalletCodeConstraint.MoneyWalletPrefix;
            }
            else
            {
                wallet.Type = WalletType.Points.ToString();
                wallet.Name = "Ví điểm của " + wallet.Name;
                walletPrefix = EntityCodeConstrant.WalletCodeConstraint.PointWalletPrefix;
            }
            wallet.Code = EntityCodeUtil.GenerateEntityCode(walletPrefix, await _repository.CountAsync() + 1);
            await _repository.InsertAsync(wallet);
            await _unitOfWork.CommitAsync();
        }
        public async Task<Wallet> GetMoneyWalletByUserId(Guid userId)
        {
            var filters = new List<Expression<Func<Wallet, bool>>>
            {
                p => p.UserId == userId && p.Type == WalletType.Money.ToString() && p.ProfileId == null
            };
            var result = await _repository.FirstOrDefaultAsync(filters);
            return result!;
        }
        public Task<Wallet> GetByIdAsync(Guid walletId)
        {
            var filters = new List<Expression<Func<Wallet, bool>>>
            {
                p => p.Id == walletId
            };
            var result = _repository.FirstOrDefaultAsync(BaseEntityStatus.Active, filters)
                ?? throw new EntityNotFoundException(MessageConstants.WalletMessageConstrant.WalletNotFound(walletId));
            return result!;
        }

        public async Task<ICollection<GetWalletByCurrentCustomerAndProfileResponse>> GetWalletByCurrentCustomerAndProfileAsync(Guid customerId, Guid? profileId)
        {
            List<Expression<Func<Wallet, bool>>> filters = new()
            {
                p => p.UserId == customerId
            };
            if (profileId != null) filters.Add(p => p.ProfileId == profileId);
            var wallets = await _repository.GetListAsync<GetWalletByCurrentCustomerAndProfileResponse>(
                status: BaseEntityStatus.Active,
                filters: filters);
            return wallets;
        }

        public async Task<GetWalletTypeMoneyByCustomerId> GetWalletTypeMoneyByCustomerIdAsync(Guid customerId)
        {
            List<Expression<Func<Wallet, bool>>> filters = new()
            {
                p => p.UserId == customerId && WalletType.Money.ToString().Equals(p.Type)
            };
            var wallet = await _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active, filters: filters);
            return _mapper.Map<GetWalletTypeMoneyByCustomerId>(wallet);
        }

        public async Task UpdateAsync(Wallet wallet)
        {
            await _repository.UpdateAsync(wallet);
            await _unitOfWork.CommitAsync();
        }
    }
}
