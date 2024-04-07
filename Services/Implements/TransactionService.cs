using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Transaction.Request;
using DataTransferObjects.Models.Transaction.Response;
using DataTransferObjects.Models.VnPay.Request;
using Google.Apis.Util;
using Microsoft.AspNetCore.Http;
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

    public class TransactionService : BaseService<Transaction>, ITransactionService
    {
        private readonly IVnPayService _vnPayService;
        private readonly IWalletService _walletService;
        private readonly IProfileService _profileService;



        public TransactionService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, IVnPayService vnPayService, IWalletService walletService, IProfileService profileService) : base(unitOfWork, mapper, appSettings)
        {
            _vnPayService = vnPayService;
            _walletService = walletService;
            _profileService = profileService;
        }
        public async Task CreateTransactionAsync(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            transaction.Status = OrderActivityStatus.Active;
            transaction.Id = Guid.NewGuid();
            transaction.Time = TimeUtil.GetCurrentVietNamTime();    
            await _repository.InsertAsync(transaction);
            await _unitOfWork.CommitAsync();
        }

        public async Task CreateTransactionListAsync(List<Transaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                await CreateTransactionAsync(transaction);
            }
        }

        public string CreateVnPayPaymentRequest(User user, int amount, HttpContext context)
        {
            Console.WriteLine(amount);
            var wallet = user.Wallets!.FirstOrDefault(w => WalletType.Money.ToString().Equals(w.Type));
            var vnPayEntity = new VnPayRequest
            {
                WalletId = wallet!.Id,
                Description = "Nap tien",
                Amount = amount,
                CreatedDate = DateTime.Now,
                FullName = user.FullName!,
            };
            return _vnPayService.CreatePaymentUrl(context, vnPayEntity);
        }


        public async Task CreateTopUpTransactionAsync(string walletId, string amount)
        {
            var walletIdGuid = Guid.Parse(walletId);
            var wallet = await _walletService.GetByIdAsync(walletIdGuid);
            var amountDouble = double.Parse(amount) / 100;
            var transaction = new Transaction
            {
                WalletId = walletIdGuid,
                Value = amountDouble,
                Id = Guid.NewGuid(),
                Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.TransactionCodeConstrant.TransactionPrefix, await _repository.CountAsync() + 1)
            };
            await CreateTransactionAsync(transaction);
            wallet.Balance += amountDouble;
            await _walletService.UpdateAsync(wallet);
            //await _unitOfWork.CommitAsync();
        }

        public async Task<IPaginable<GetTransactionPageByProfileIdAndCurrentUserResponse>> GetTransactionPageByProfileIdAndCurrentUser(Guid profileId, PaginationRequest paginationRequest, User user)
        {
            var profile = await _profileService.GetByIdAsync(profileId);
            if (profile.UserId != user.Id)
            {
                throw new InvalidRequestException(MessageConstants.ProfileMessageConstrant.ProfileDoesNotBelongToUser);
            }
            List<Expression<Func<Transaction, bool>>> filters = new List<Expression<Func<Transaction, bool>>>()
            {

                t => (t.OrderId == null && t.GameId == null && t.ExchangeGiftId == null) ? t.Wallet!.UserId == user.Id : t.Wallet!.User!.Profiles!.Any(p => p.Id == profileId && p.Status == BaseEntityStatus
                .Active),
            };
            var transactionPage = await _repository.GetPageAsync<GetTransactionPageByProfileIdAndCurrentUserResponse>(paginationRequest, filters);
            return transactionPage;
        }
    }
}
