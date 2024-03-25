using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.ExchangeGift;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ExchangeGiftService : BaseService<ExchangeGift>, IExchangeGIftService
    {

        private readonly IGiftService _giftService;
        private readonly IProfileService _profileService;
        private readonly ISessionDetailService _sessionDetailService;
        private readonly ITransactionService _transactionService;
        public ExchangeGiftService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, IGiftService giftService, IProfileService profileService, ISessionDetailService sessionDetailService, ITransactionService transactionService) : base(unitOfWork, mapper, appSettings)
        {
            _giftService = giftService;
            _profileService = profileService;
            _sessionDetailService = sessionDetailService;
            _transactionService = transactionService;
        }

        public async Task CreateExchangeGiftAsync(ExchangeGiftRequest request, Guid customerId)
        {
            var gift = await _giftService.GetGiftByIdAsync(request.GiftId);
            var profile = await _profileService.GetProfileByIdAndCurrentCustomerIdAsync(request.ProfileId, customerId);
            var sessionDetail = await _sessionDetailService.GetByIdAsync(request.SessionDetailId);
            if (sessionDetail.Session!.OrderEndTime.CompareTo(DateTime.Now) <= 0)
            {
                throw new ClosedSessionException();
            }
            if (sessionDetail.Location!.SchoolId != profile.SchoolId)
            {
                throw new InvalidSchoolException();
            }
            var exchangeGift = _mapper.Map<ExchangeGift>(request);
            exchangeGift.Id = Guid.NewGuid();
            exchangeGift.Points = gift.Points;
            var wallet = profile.Wallets!.FirstOrDefault(w => w.Type == WalletType.Points.ToString())!;
            if (wallet.Balance - gift.Points < 0)
            {
                throw new InvalidWalletBalanceException(MessageConstants.WalletMessageConstrant.NotEnoughPoints);
            }
            exchangeGift.PaymentDate = DateTime.Now;
            exchangeGift.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.ExchangeGiftCodeConstraint.ExchangeGiftPrefix, await _repository.CountAsync());
            exchangeGift.Transactions = new List<Transaction>
            {
                new Transaction
                {
                    Id = Guid.NewGuid(),
                    Value = -gift.Points,
                    Time = DateTime.Now,
                    WalletId = wallet.Id,
                    Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.TransactionCodeConstrant.ExchangeGiftTransactionPrefix, await _transactionService.CountAsync() + 1),

                }
            };
            exchangeGift.Activities = new List<OrderActivity>
            {
                new OrderActivity
                {
                    Id = Guid.NewGuid(),
                    Name = MessageConstants.OrderActivityMessageConstrant.DefaultExchangeGiftCreatedActivityName,
                    Time = DateTime.Now,
                    ImagePath = null,
                    Status = BaseEntityStatus.Active,
                    Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, await _transactionService.CountAsync() + 1)
                }
            };

            wallet.Balance -= gift.Points;

            await _repository.InsertAsync(exchangeGift);
            await _unitOfWork.CommitAsync();
            //await Console.Out.WriteLineAsync(sessionDetail.ToString());
        }

    }
}
