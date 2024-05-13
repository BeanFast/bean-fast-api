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

        private readonly IWalletRepository _repository;
        public WalletService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, IWalletRepository repository) : base(unitOfWork, mapper, appSettings)
        {
            _repository = repository;
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
            return await _repository.GetMoneyWalletByUserId(userId);
        }
        public async Task<Wallet> GetByIdAsync(Guid walletId)
        {
            return await _repository.GetByIdAsync(walletId);
        }

        public async Task<ICollection<GetWalletByCurrentCustomerAndProfileResponse>> GetWalletByCurrentCustomerAndProfileAsync(Guid customerId, Guid? profileId)
        {
            return await _repository.GetWalletByCurrentCustomerAndProfileAsync(customerId, profileId);
        }

        public async Task<GetWalletTypeMoneyByCustomerId> GetWalletTypeMoneyByCustomerIdAsync(Guid customerId)
        {
            return await _repository.GetWalletTypeMoneyByCustomerIdAsync(customerId);
        }

        public async Task UpdateAsync(Wallet wallet)
        {
            await _repository.UpdateAsync(wallet);
            await _unitOfWork.CommitAsync();
        }

        public async Task<Wallet> GetPointWalletByUserIdAndProfildId(Guid userId, Guid profileId)
        {
            var wallet = await _repository.GetPointWalletByUserIdAndProfildId(userId, profileId);
            return wallet!;
        }


    }
}
