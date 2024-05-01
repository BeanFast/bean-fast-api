using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.ExchangeGift.Request;
using DataTransferObjects.Models.ExchangeGift.Response;
using DataTransferObjects.Models.Order.Request;
using DataTransferObjects.Models.Order.Response;
using DataTransferObjects.Models.OrderActivity.Request;
using DataTransferObjects.Models.OrderActivity.Response;
using Microsoft.EntityFrameworkCore;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Services.Implements
{
    public class ExchangeGiftService : BaseService<ExchangeGift>, IExchangeGIftService
    {

        private readonly IGiftService _giftService;
        private readonly IProfileService _profileService;
        private readonly ISessionDetailService _sessionDetailService;
        private readonly ITransactionService _transactionService;
        private readonly IOrderActivityService _orderActivityService;
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;

        public ExchangeGiftService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, IGiftService giftService, IProfileService profileService, ISessionDetailService sessionDetailService, ITransactionService transactionService, IOrderActivityService orderActivityService, IUserService userService, IWalletService walletService) : base(unitOfWork, mapper, appSettings)
        {
            _giftService = giftService;
            _profileService = profileService;
            _sessionDetailService = sessionDetailService;
            _transactionService = transactionService;
            _orderActivityService = orderActivityService;
            _userService = userService;
            _walletService = walletService;
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
            exchangeGift.Status = ExchangeGiftStatus.Active;
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
            await _walletService.UpdateAsync(wallet);
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
        public async Task<ExchangeGift> GetByIdAsync(Guid exchangeGiftId)
        {
            var filters = new List<Expression<Func<ExchangeGift, bool>>>
            {
                (exchangeGift) => exchangeGift.Id == exchangeGiftId
            };
            var result = await _repository.FirstOrDefaultAsync(filters: filters, include: i => i.Include(eg => eg.Profile!).ThenInclude(p => p.User!));
            return result!;
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
        public async Task<ICollection<ExchangeGift>> GetDeliveringExchangeGiftsByDelivererIdAndCustomerIdAsync(Guid delivererId, Guid customerId)
        {
            List<Expression<Func<ExchangeGift, bool>>> filters = new()
            {
                (order) => order.SessionDetail!.SessionDetailDeliverers!.Any(sdd => sdd.DelivererId == delivererId)
                && order.Profile!.UserId == customerId
                && order.Status == ExchangeGiftStatus.Delivering
            };
            var exchangeGifts = await _repository.GetListAsync(
                filters: filters,
                include: queryable => queryable
                .Include(o => o.SessionDetail!)
                .ThenInclude(sd => sd.Session!)
                .Include(o => o.Gift!)
            );
            return exchangeGifts!;
        }
        public async Task<List<GetExchangeGiftResponse>> GetValidExchangeGiftResponsesByQRCodeAsync(string qrCode, Guid delivererId)
        {
            var customer = await _userService.GetCustomerByQrCodeAsync(qrCode);
            var exchangeGifts = await GetDeliveringExchangeGiftsByDelivererIdAndCustomerIdAsync(delivererId, customer.Id);
            //if (exchangeGifts.IsNullOrEmpty())
            //{
            //    throw new EntityNotFoundException(MessageConstants.OrderMessageConstrant.NoDeliveryOrders);
            //}
            var timeScanning = TimeUtil.GetCurrentVietNamTime();
            var validExchangeGifts = new List<GetExchangeGiftResponse>();
            foreach (var exchangeGift in exchangeGifts)
            {
                if (timeScanning >= exchangeGift.SessionDetail!.Session!.DeliveryStartTime && timeScanning < exchangeGift.SessionDetail!.Session!.DeliveryEndTime)
                {

                    validExchangeGifts.Add(_mapper.Map<GetExchangeGiftResponse>(exchangeGift));
                }
            }
            //if (validExchangeGifts.Count == 0)
            //{
            //    throw new EntityNotFoundException(MessageConstants.OrderMessageConstrant.NotFoundOrders);
            //}
            return validExchangeGifts;
        }

        public async Task UpdateExchangeGiftToDeliveryStatusAsync(Guid exchangeGiftId)
        {
            var orderActivityNumber = await _orderActivityService.CountAsync() + 1;
            var exchangeGiftEntity = await GetByIdAsync(exchangeGiftId);
            exchangeGiftEntity.Status = ExchangeGiftStatus.Delivering;
            //await RollbackMoneyAsync(orderEntity);
            await _orderActivityService.CreateOrderActivityAsync(exchangeGiftEntity, new OrderActivity
            {
                Id = Guid.NewGuid(),
                Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, orderActivityNumber),
                Name = MessageConstants.OrderActivityMessageConstrant.ExchangeGiftDeliveringActivityName,
                Time = TimeUtil.GetCurrentVietNamTime(),
                Status = OrderActivityStatus.Active
            }, null!);
            await _repository.UpdateAsync(exchangeGiftEntity);
            await _unitOfWork.CommitAsync();
        }


        public async Task CancelOrderForManagerAsync(ExchangeGift exchangeGift, CancelOrderRequest request, User manager)
        {
            exchangeGift.Status = ExchangeGiftStatus.Cancelled;
            var orderActivityNumber = await _orderActivityService.CountAsync() + 1;

            var orderActivity = new OrderActivity
            {
                Id = Guid.NewGuid(),
                Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, orderActivityNumber),
                Name = request.Reason,
                Time = TimeUtil.GetCurrentVietNamTime(),
                Status = OrderActivityStatus.Active,
                OrderId = exchangeGift.Id
            };
            await RollbackPointsAsync(exchangeGift);
            await _orderActivityService.CreateOrderActivityAsync(exchangeGift, orderActivity, manager);
            await _repository.UpdateAsync(exchangeGift, manager);
            await _unitOfWork.CommitAsync();
        }
        //task: update exchange gift status
        public async Task RollbackPointsAsync(ExchangeGift exchangeGift)
        {
            var moneyWallet = exchangeGift.Profile!.Wallets!.FirstOrDefault(w => WalletType.Points.ToString().Equals(w.Type.ToString()));
            moneyWallet!.Balance += exchangeGift.Points;
            var rollbackMoneyTransaction = new Transaction
            {
                OrderId = exchangeGift.Id,
                Id = Guid.NewGuid(),
                Value = exchangeGift.Points,
                WalletId = moneyWallet!.Id,
                Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.TransactionCodeConstrant.ExchangeGiftTransactionPrefix, await _transactionService.CountAsync() + 1)
            };
            await _walletService.UpdateAsync(moneyWallet);
            await _transactionService.CreateTransactionAsync(rollbackMoneyTransaction);
        }
    }
}
