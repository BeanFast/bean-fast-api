using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.ExchangeGift.Request;
using DataTransferObjects.Models.ExchangeGift.Response;
using DataTransferObjects.Models.OrderActivity.Request;
using DataTransferObjects.Models.OrderActivity.Response;
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
        public List<Expression<Func<ExchangeGift, bool>>> GetFilterFromFilterRequest(ExchangeGiftFilterRequest filterRequest)
        {
            List<Expression<Func<ExchangeGift, bool>>> expressions = new List<Expression<Func<ExchangeGift, bool>>>();
            if(filterRequest.Status != null)
            {
                expressions.Add(eg => eg.Status == filterRequest.Status);
            }
            if (!filterRequest.Code.IsNullOrEmpty())
            {
                expressions.Add(eg => eg.Code == filterRequest.Code);
            }
            return expressions;
        }
        public async Task CreateExchangeGiftAsync(CreateExchangeGiftRequest request, User user)
        {
            var gift = await _giftService.GetGiftByIdAsync(request.GiftId);
            var profile = await _profileService.GetProfileByIdAndCurrentCustomerIdAsync(request.ProfileId, user.Id);
            var sessionDetail = await _sessionDetailService.GetByIdAsync(request.SessionDetailId);
            if (sessionDetail.Session!.OrderEndTime < TimeUtil.GetCurrentVietNamTime())
            {
                throw new InvalidRequestException(MessageConstants.SessionDetailMessageConstrant.SessionOrderClosed);
            }else if (sessionDetail.Session!.OrderStartTime >= TimeUtil.GetCurrentVietNamTime())
            {
                throw new InvalidRequestException(MessageConstants.SessionDetailMessageConstrant.SessionOrderNotStarted);
            }
            if (sessionDetail.Location!.SchoolId != profile.SchoolId)
            {
                throw new InvalidRequestException(MessageConstants.SessionDetailMessageConstrant.InvalidSchoolLocation);
            }
            gift.InStock -= 1;
            if(gift.InStock < 0) throw new InvalidRequestException("Món quà này đã hết hàng");
            
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
            await _giftService.UpdateGiftAsync(gift);
            await _repository.InsertAsync(exchangeGift, user);
            await _unitOfWork.CommitAsync();
            //await Console.Out.WriteLineAsync(sessionDetail.ToString());
        }

        public async Task<ICollection<GetOrderActivityResponse>> GetOrderActivitiesByExchangeGiftIdAsync(Guid exchangeGiftId, User user)
        {
            return await _orderActivityService.GetOrderActivitiesByExchangeGiftIdAsync(exchangeGiftId, user);
        }
        public async Task CreateOrderActivityAsync(CreateOrderActivityRequest request, User user)
        {
            request.OrderId = null;
            if (request.ExchangeGiftId == null)
            {
                throw new InvalidRequestException(MessageConstants.ExchangeGiftMessageConstrant.ExchangeGiftIdRequired);
            }
            await _orderActivityService.CreateOrderActivityAsync(request, user);
        }

        public async Task<IPaginable<GetExchangeGiftResponse>> GetExchangeGiftsAsync(ExchangeGiftFilterRequest filterRequest, PaginationRequest paginationRequest)
        {
            var filters = GetFilterFromFilterRequest(filterRequest);
            return await _repository.GetPageAsync<GetExchangeGiftResponse>(filters: filters, paginationRequest: paginationRequest, orderBy: o => o.OrderByDescending(eg => eg.CreatedDate));
        }

        public async Task<IPaginable<GetExchangeGiftResponse>> GetExchangeGiftsByCurrentCustomerAndProfileIdAsync(ExchangeGiftFilterRequest filterRequest, PaginationRequest paginationRequest, User user, Guid profileId)
        {
            await _profileService.GetProfileByIdAndCurrentCustomerIdAsync(profileId, user.Id);
            var filters = GetFilterFromFilterRequest(filterRequest);
            filters.Add(eg => eg.ProfileId == profileId);
            IPaginable<GetExchangeGiftResponse> page =  await _repository.GetPageAsync<GetExchangeGiftResponse>(filters: filters, paginationRequest: paginationRequest, orderBy: o => o.OrderByDescending(eg => eg.CreatedDate));
            return page;
        }
    }
}
