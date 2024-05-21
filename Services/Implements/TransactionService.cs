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
using Microsoft.IdentityModel.Tokens;
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
        private readonly IGameService _gameService;
        private readonly ITransactionRepository _repository;

        public TransactionService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, IVnPayService vnPayService, IWalletService walletService, IProfileService profileService, IGameService gameService, ITransactionRepository repository) : base(unitOfWork, mapper, appSettings)
        {
            _vnPayService = vnPayService;
            _walletService = walletService;
            _profileService = profileService;
            _gameService = gameService;
            _repository = repository;
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
       

        public async Task<IPaginable<GetTransactionPageByProfileIdAndCurrentUserResponse>> GetTransactionPageByProfileIdAndCurrentUser(
            Guid profileId,
            PaginationRequest paginationRequest,
            TransactionFilterRequest filterRequest,
            User user)
        {
            var profile = await _profileService.GetByIdAsync(profileId);
            if (profile.UserId != user.Id)
            {
                throw new InvalidRequestException(MessageConstants.ProfileMessageConstrant.ProfileDoesNotBelongToUser);
            }
           
            var transactionPage = await _repository.GetTransactionPageByProfileIdAndUserAsync(profileId, paginationRequest, filterRequest, user);
            return transactionPage;
        }
        public async Task<IPaginable<GetTransactionPageByCurrentUserResponse>> GetMoneyTransactionPageByCurrentUser(
            PaginationRequest paginationRequest, 
            TransactionFilterRequest filterRequest, 
            User user)
        {
            var transactionPage = await _repository.GetMoneyTransactionPageByUserAsync(paginationRequest, filterRequest, user);
            return transactionPage;
        }

        public async Task<ICollection<GetTransactionsForDashBoardResponse>> GetTransactionsForDashBoard(GetTransactionsForDashBoardRequest request)
        {
            return await _repository.GetTransactionsForDashBoardAsync(request);
            //var filters = new List<Expression<Func<Transaction, bool>>>();
            //if (request.Type == "money")
            //{
            //    filters.Add(t => t.OrderId == null && t.GameId == null && t.ExchangeGiftId == null);
            //}
            //if (request.StartDate != DateTime.MinValue)
            //{
            //    filters.Add(t => t.Time.Date >= request.StartDate.Date && t.Time.Date <= request.EndDate.Date);
            //}
            //var transactions = await _repository.GetListAsync(filters);
            //return transactions.GroupBy(t => t.Time.Month)
            //    .OrderBy(group => group.Key)
            //    .Select(group => new GetTransactionsForDashBoardResponse
            //{
            //    Count = group.Count(),
            //    Month = TimeUtil.GetMonthName(group.Key),
            //    Value = (int)group.Sum(t => t.Value)
            //}).ToList();
        }
        public async Task<int> GetPlayedGameCount(User user)
        {
            return await _repository.GetPlayedGameCountAsync(user);
        }
        public async Task<int> GetRemainingPlayGameCount(User user)
        {
            var playedGameCount = await GetPlayedGameCount(user);
            return TransactionConstrant.MaxGameTransactionPerDay - playedGameCount;
        }

        public async Task CreateGameTransactionAsync(CreateGameTransactionRequest request, User user)
        {
            await _gameService.GetGamesAsync();
            var playedGameTransactions = await GetPlayedGameCount(user);
            if (playedGameTransactions >= TransactionConstrant.MaxGameTransactionPerDay)
            {
                throw new InvalidRequestException(MessageConstants.TransactionMessageConstrant.GameTransactionIsExceedPermittedAmount);
            }
            var wallet = await _walletService.GetPointWalletByUserIdAndProfildId(user.Id);

            var transaction = new Transaction
            {
                Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.TransactionCodeConstrant.TransactionPrefix, await _repository.CountAsync() + 1),
                GameId = request.GameId,
                Value = request.Points,
                Time = TimeUtil.GetCurrentVietNamTime(),
                WalletId = wallet.Id,
                Status = BaseEntityStatus.Active
            };
            wallet.Balance += request.Points;
            await _walletService.UpdateAsync(wallet);
            await _repository.InsertAsync(transaction, user);
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
            if (amount < 10000)
            {
                throw new InvalidRequestException(MessageConstants.TransactionMessageConstrant.TopUpMoneyMustBeGreaterThanTenThousand);
            }
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
    }
}
