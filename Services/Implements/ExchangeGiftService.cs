using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.ExchangeGift.Request;
using DataTransferObjects.Models.OrderActivity.Request;
using DataTransferObjects.Models.OrderActivity.Response;
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
        private readonly IOrderActivityService _orderActivityService;
        public ExchangeGiftService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, IGiftService giftService, IProfileService profileService, ISessionDetailService sessionDetailService, ITransactionService transactionService, IOrderActivityService orderActivityService) : base(unitOfWork, mapper, appSettings)
        {
            _giftService = giftService;
            _profileService = profileService;
            _sessionDetailService = sessionDetailService;
            _transactionService = transactionService;
            _orderActivityService = orderActivityService;
        }

        public async Task CreateExchangeGiftAsync(CreateExchangeGiftRequest request, Guid customerId)
        {
            var gift = await _giftService.GetGiftByIdAsync(request.GiftId);
            var profile = await _profileService.GetProfileByIdAndCurrentCustomerIdAsync(request.ProfileId, customerId);
            var sessionDetail = await _sessionDetailService.GetByIdAsync(request.SessionDetailId);
            if (sessionDetail.Session!.OrderEndTime.CompareTo(TimeUtil.GetCurrentVietNamTime()) <= 0)
            {
                throw new(MessageConstants.SessionDetailMessageConstrant.SessionOrderClosed);
            }
            if (sessionDetail.Location!.SchoolId != profile.SchoolId)
            {
                throw new InvalidRequestException(MessageConstants.SessionDetailMessageConstrant.InvalidSchoolLocation);
            }
            var exchangeGift = _mapper.Map<ExchangeGift>(request);
            exchangeGift.Id = Guid.NewGuid();
            exchangeGift.Points = gift.Points;
            var wallet = profile.Wallets!.FirstOrDefault(w => w.Type == WalletType.Points.ToString())!;
            if (wallet.Balance - gift.Points < 0)
            {
                throw new InvalidWalletBalanceException(MessageConstants.WalletMessageConstrant.NotEnoughPoints);
            }
            wallet.Balance -= gift.Points;
            exchangeGift.PaymentDate = TimeUtil.GetCurrentVietNamTime();
            exchangeGift.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.ExchangeGiftCodeConstraint.ExchangeGiftPrefix, await _repository.CountAsync());
            exchangeGift.Transactions = new List<Transaction>
            {
                new Transaction
                {
                    Id = Guid.NewGuid(),
                    Value = -gift.Points,
                    Time = TimeUtil.GetCurrentVietNamTime(),
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
                    Time = TimeUtil.GetCurrentVietNamTime(),
                    ImagePath = null,
                    Status = BaseEntityStatus.Active,
                    Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, await _transactionService.CountAsync() + 1)
                }
            };
            await _repository.InsertAsync(exchangeGift);
            await _unitOfWork.CommitAsync();
            //await Console.Out.WriteLineAsync(sessionDetail.ToString());
        }

        public async Task<ICollection<GetOrderActivityResponse>> GetOrderActivitiesByExchangeGiftIdAsync(Guid exchangeGiftId, User user)
        {
            return await _orderActivityService.GetOrderActivitiesByExchangeGiftIdAsync(exchangeGiftId, user);
        }
        public async Task CreateOrderActivityAsync(CreateOrderActivityRequest request)
        {
            request.OrderId = null;
            if (request.ExchangeGiftId == null)
            {
                throw new InvalidRequestException(MessageConstants.ExchangeGiftMessageConstrant.ExchangeGiftIdRequired);
            }
            await _orderActivityService.CreateOrderActivityAsync(request);
        }
    }
}
