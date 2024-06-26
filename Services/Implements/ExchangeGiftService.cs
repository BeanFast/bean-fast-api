﻿using AutoMapper;
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
        private readonly IExchangeGiftRepository _repository;
        //private readonly ISessionDetailDelivererService _sessionDetailDelivererService;
        private readonly ISessionDetailDelivererRepository _sessionDetailDelivererRepository;

        public ExchangeGiftService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, IGiftService giftService, IProfileService profileService, ISessionDetailService sessionDetailService, ITransactionService transactionService, IOrderActivityService orderActivityService, IUserService userService, IWalletService walletService, IExchangeGiftRepository repository,
            //ISessionDetailDelivererService sessionDetailDelivererService,
            ISessionDetailDelivererRepository sessionDetailDelivererRepository) : base(unitOfWork, mapper, appSettings)
        {
            _giftService = giftService;
            _profileService = profileService;
            _sessionDetailService = sessionDetailService;
            _transactionService = transactionService;
            _orderActivityService = orderActivityService;
            _userService = userService;
            _walletService = walletService;
            _repository = repository;
            //_sessionDetailDelivererService = sessionDetailDelivererService;
            _sessionDetailDelivererRepository = sessionDetailDelivererRepository;
        }

        public async Task CreateExchangeGiftAsync(CreateExchangeGiftRequest request, User user)
        {
            var gift = await _giftService.GetGiftByIdAsync(request.GiftId);
            var profile = await _profileService.GetProfileByIdAndCurrentCustomerIdAsync(request.ProfileId, user.Id);
            var sessionDetail = await _sessionDetailService.GetByIdAsync(request.SessionDetailId);
            if (sessionDetail.Session!.OrderEndTime < TimeUtil.GetCurrentVietNamTime())
            {
                throw new InvalidRequestException(MessageConstants.SessionDetailMessageConstrant.SessionOrderClosed);
            }
            else if (sessionDetail.Session!.OrderStartTime >= TimeUtil.GetCurrentVietNamTime())
            {
                throw new InvalidRequestException(MessageConstants.SessionDetailMessageConstrant.SessionOrderNotStarted);
            }
            if (sessionDetail.Location!.SchoolId != profile.SchoolId)
            {
                throw new InvalidRequestException(MessageConstants.SessionDetailMessageConstrant.InvalidSchoolLocation);
            }
            gift.InStock -= 1;
            if (gift.InStock < 0) throw new InvalidRequestException(MessageConstants.ExchangeGiftMessageConstrant.GiftOutOfStock);

            var exchangeGift = _mapper.Map<ExchangeGift>(request);
            exchangeGift.Id = Guid.NewGuid();
            exchangeGift.Points = gift.Points;
            exchangeGift.Status = ExchangeGiftStatus.Active;
            var wallet = profile.User!.Wallets!.FirstOrDefault(w => w.Type == WalletType.Points.ToString())!;
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
            await AssignExchangeGiftToDelivererAsync(exchangeGift, user);
            await _repository.InsertAsync(exchangeGift, user);
            await _unitOfWork.CommitAsync();
            //await Console.Out.WriteLineAsync(sessionDetail.ToString());
        }
        public async Task AssignExchangeGiftToDelivererAsync(ExchangeGift exchangeGift, User customer)
        {
            var availableDeliverers = await _sessionDetailDelivererRepository.GetBySessionDetailId(exchangeGift.SessionDetailId);
            var data = await _repository.GetDelivererIdAndOrderCountBySessionDetailId(exchangeGift.SessionDetailId);
            if (availableDeliverers.IsNullOrEmpty())
            {
                throw new InvalidRequestException(MessageConstants.SessionDetailMessageConstrant.NoDelivererAvailableInThisSession);
            }
            var delivererThatAlreadyHasOrderOfThisCustomer = data.FirstOrDefault(d => d.CustomerIds.Any(id => id == customer.Id));
            if (delivererThatAlreadyHasOrderOfThisCustomer != null)
            {
                exchangeGift.DelivererId = delivererThatAlreadyHasOrderOfThisCustomer.DelivererId;
                return;
            }
            foreach (var sessionDetailDeliverer in availableDeliverers)
            {
                if (!data.Any(d => d.DelivererId == sessionDetailDeliverer.DelivererId))
                {
                    data.Add(new GetDelivererIdAndOrderCountBySessionDetailIdResponse { DelivererId = sessionDetailDeliverer.DelivererId });
                }
            }
            if (!data.IsNullOrEmpty())
            {
                var sortedData = data.OrderBy(d => d.OrderCount).ToList();
                exchangeGift.DelivererId = sortedData.First().DelivererId;
            }

            Console.WriteLine(availableDeliverers);
        }
        public async Task AssignExchangeGiftToDelivererAndUpdateAsync(ExchangeGift exchangeGift, User customer)
        {
            await AssignExchangeGiftToDelivererAsync(exchangeGift, customer);
            await _repository.UpdateAsync(exchangeGift);
            await _unitOfWork.CommitAsync();
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
            return await _repository.GetByIdAsync(exchangeGiftId);
        }

        public async Task<ExchangeGift> GetByIdIncludeDeliverersAsync(Guid exchangeGiftId)
        {
            var result = await _repository.GetByIdIncludeDeliverersAsync(exchangeGiftId) 
                ?? throw new EntityNotFoundException(MessageConstants.ExchangeGiftMessageConstrant.ExchangeGiftNotFound(exchangeGiftId));
            return result;
        }
        public async Task<ICollection<ExchangeGift>> GetBySessionDetailId(Guid sessionDetailId)
        {
            return await _repository.GetBySessionDetailId(sessionDetailId);
        }
        public async Task<IPaginable<GetExchangeGiftResponse>> GetExchangeGiftsAsync(ExchangeGiftFilterRequest filterRequest, PaginationRequest paginationRequest, User user)
        {
            return await _repository.GetExchangeGiftsAsync(filterRequest, paginationRequest, user);
        }

        public async Task<IPaginable<GetExchangeGiftResponse>> GetExchangeGiftsByCurrentCustomerAndProfileIdAsync(ExchangeGiftFilterRequest filterRequest, PaginationRequest paginationRequest, User user, Guid profileId)
        {
            await _profileService.GetProfileByIdAndCurrentCustomerIdAsync(profileId, user.Id);
            return await _repository.GetExchangeGiftsByCurrentCustomerAndProfileIdAsync(filterRequest, paginationRequest, user, profileId);
        }

        public async Task<List<GetExchangeGiftResponse>> GetValidExchangeGiftResponsesByQRCodeAsync(string qrCode, Guid delivererId)
        {
            var customer = await _userService.GetCustomerByQrCodeAsync(qrCode);
            var exchangeGifts = await _repository.GetDeliveringExchangeGiftsByDelivererIdAndCustomerIdAsync(delivererId, customer.Id);
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
        public async Task<GetExchangeGiftResponse> GetExchangeGiftResponseByIdAsync(Guid id)
        {
            var filters = new List<Expression<Func<ExchangeGift, bool>>>
            {
                (exchangeGift) => exchangeGift.Id == id
            };
            var result = await _repository.FirstOrDefaultAsync<GetExchangeGiftResponse>(filters: filters);
            return result!;
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

        public async Task CancelExchangeGiftAsync(Guid exchangeGiftId, CancelExchangeGiftRequest request, User user)
        {
            var exchangeGift = await GetByIdAsync(exchangeGiftId);
            if (OrderStatus.Completed == exchangeGift.Status)
            {
                throw new InvalidRequestException(MessageConstants.ExchangeGiftMessageConstrant.ExchangeGiftCannotBeCancelInCompleteStatus);
            }
            if (RoleName.CUSTOMER.ToString() == user.Role!.EnglishName)
            {
                await CancelExchangeGiftForCustomerAsync(exchangeGift, request, user);
            }
            else if (RoleName.MANAGER.ToString() == user.Role.EnglishName)
            {
                if (ExchangeGiftStatus.Cancelled == exchangeGift.Status || ExchangeGiftStatus.CancelledByCustomer == exchangeGift.Status)
                {
                    throw new InvalidRequestException(MessageConstants.ExchangeGiftMessageConstrant.ExchangeGiftCanceled);
                }
                await CancelExchangeGiftForManagerAsync(exchangeGift, request, user);
            }
        }

        public async Task CancelExchangeGiftForManagerAsync(ExchangeGift exchangeGift, CancelExchangeGiftRequest request, User manager)
        {
            exchangeGift.Status = ExchangeGiftStatus.Cancelled;
            var orderActivityNumber = await _orderActivityService.CountAsync() + 1;

            var orderActivity = new OrderActivity
            {
                Id = Guid.NewGuid(),
                Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, orderActivityNumber),
                Name = MessageConstants.OrderActivityMessageConstrant.ExchangeGiftCanceledByCustomerActivityName(request.Reason),//request.Reason,
                Time = TimeUtil.GetCurrentVietNamTime(),
                Status = OrderActivityStatus.Active,
                ExchangeGiftId = exchangeGift.Id
            };
            await RollbackPointsAsync(exchangeGift);
            await _orderActivityService.CreateOrderActivityAsync(exchangeGift, orderActivity, manager);
            await _repository.UpdateAsync(exchangeGift, manager);
            await _unitOfWork.CommitAsync();
        }

        public async Task CancelExchangeGiftForCustomerAsync(ExchangeGift exchangeGift, CancelExchangeGiftRequest request, User customer)
        {
            var validTime = TimeUtil.GetCurrentVietNamTime();
            var orderActivityNumber = await _orderActivityService.CountAsync() + 1;
            if (exchangeGift.Profile!.User!.Id != customer.Id)
            {
                throw new InvalidRequestException(MessageConstants.ExchangeGiftMessageConstrant.ExchangeGiftNotBelongToThisUser);
            }
            if (validTime >= exchangeGift.SessionDetail!.Session!.OrderStartTime &&
                validTime < exchangeGift.SessionDetail!.Session!.OrderEndTime)
            {
                // hoan tien
                //var moneyWallet = exchangeGift.Profile!.User!.Wallets!.FirstOrDefault(w => WalletType.Money.ToString().Equals(w.Type));
                //if (moneyWallet != null)
                //{
                await RollbackPointsAsync(exchangeGift);
                //}
            }
            var orderActivity = new OrderActivity
            {
                Id = Guid.NewGuid(),
                Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, orderActivityNumber),
                Name = MessageConstants.OrderActivityMessageConstrant.ExchangeGiftCanceledByCustomerActivityName(request.Reason),
                Time = TimeUtil.GetCurrentVietNamTime(),
                Status = OrderActivityStatus.Active
            };
            await _orderActivityService.CreateOrderActivityAsync(exchangeGift, orderActivity, customer);
            exchangeGift.Status = ExchangeGiftStatus.CancelledByCustomer;
            exchangeGift.Profile = null;
            await _repository.UpdateAsync(exchangeGift, customer);
            await _unitOfWork.CommitAsync();
        }
        public async Task UpdateExchangeGiftCompleteStatusAsync(Guid exchangeGiftId, User deliverer)
        {
            var startTime = DateTime.Now;
            var orderActivityNumber = await _orderActivityService.CountAsync() + 1;
            var transactionNumber = await _transactionService.CountAsync() + 1;
            var exchangeGift = await GetByIdIncludeDeliverersAsync(exchangeGiftId);
            if (exchangeGift.Status != ExchangeGiftStatus.Delivering)
            {
                throw new InvalidRequestException(MessageConstants.OrderMessageConstrant.OrderNotInDeliveryStatus);
            }
            if (TimeUtil.GetCurrentVietNamTime() > exchangeGift.SessionDetail!.Session!.DeliveryEndTime)
            {
                throw new InvalidRequestException(MessageConstants.OrderMessageConstrant.OrderOutOfDeliveryTime);
            }
            if (TimeUtil.GetCurrentVietNamTime() < exchangeGift.SessionDetail!.Session!.DeliveryStartTime)
            {
                throw new InvalidRequestException(MessageConstants.OrderMessageConstrant.OrderNotInDeliveryTime);
            }
            if (!exchangeGift.SessionDetail.SessionDetailDeliverers!.Any(sdd => sdd.DelivererId == deliverer.Id))
            {
                throw new InvalidRequestException(MessageConstants.OrderMessageConstrant.CurrentUserAreNotDeliverer);
            }
            exchangeGift.Status = ExchangeGiftStatus.Completed;
            exchangeGift.DeliveryDate = TimeUtil.GetCurrentVietNamTime();
            var newOrderActivity = new OrderActivity
            {
                Id = Guid.NewGuid(),
                Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, orderActivityNumber),
                Name = MessageConstants.OrderActivityMessageConstrant.ExchangeGiftCompletedActivityName,
                Time = TimeUtil.GetCurrentVietNamTime(),
                Status = OrderActivityStatus.Active
            };
            await _orderActivityService.CreateOrderActivityAsync(exchangeGift, newOrderActivity, deliverer);
            await _repository.UpdateAsync(exchangeGift);
            await _unitOfWork.CommitAsync();
            var endTime = DateTime.Now;
            var delay = endTime - startTime;
            Console.WriteLine($"Delay: {delay.TotalMilliseconds} milliseconds");
        }
        //task: update exchange gift status
        public async Task RollbackPointsAsync(ExchangeGift exchangeGift)
        {
            //var moneyWallet = exchangeGift.Profile!.Wallets!.FirstOrDefault(w => WalletType.Points.ToString().Equals(w.Type.ToString()));
            //moneyWallet!.Balance += exchangeGift.Points;
            var pointWallet = await _walletService.GetPointWalletByUserIdAndProfildId(exchangeGift.Profile!.UserId);
            pointWallet!.Balance += exchangeGift.Points;
            var rollbackPointTransaction = new Transaction
            {
                ExchangeGiftId = exchangeGift.Id,
                Id = Guid.NewGuid(),
                Value = exchangeGift.Points,
                WalletId = pointWallet!.Id,
                Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.TransactionCodeConstrant.ExchangeGiftTransactionPrefix, await _transactionService.CountAsync() + 1)
            };
            await _walletService.UpdateAsync(pointWallet);
            await _transactionService.CreateTransactionAsync(rollbackPointTransaction);
        }

        
    }
}
