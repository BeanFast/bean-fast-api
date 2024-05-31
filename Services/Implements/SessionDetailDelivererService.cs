using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Order.Response;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repositories.Implements;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Exceptions;
using Utilities.Settings;

namespace Services.Implements
{
    public class SessionDetailDelivererService : BaseService<SessionDetailDeliverer>, ISessionDetailDelivererService
    {
        private readonly ISessionDetailDelivererRepository _delivererRepository;
        //private readonly IOrderService _orderService;
        //private readonly IExchangeGIftService _exchangeGIftService;
        private readonly IOrderRepository _orderRepository;
        private readonly IExchangeGiftRepository _exchangeGiftRepository;
        public SessionDetailDelivererService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, ISessionDetailDelivererRepository delivererRepository
            , IOrderRepository orderRepository
            , IExchangeGiftRepository exchangeGiftRepository
            //,IOrderService orderService, IExchangeGIftService exchangeGIftService
            ) : base(unitOfWork, mapper, appSettings)
        {
            _delivererRepository = delivererRepository;
            _orderRepository = orderRepository;
            _exchangeGiftRepository = exchangeGiftRepository;
        }

        public async Task HardDeleteAsync(List<SessionDetailDeliverer> sessionDetailDeliverers)
        {
            await _delivererRepository.HardDeleteRangeAsync(sessionDetailDeliverers);
        }

        public async Task InsertListAsync(List<SessionDetailDeliverer> sessionDetailDeliverers)
        {
            foreach (var item in sessionDetailDeliverers)
            {
                await _delivererRepository.InsertAsync(item);
                //await _unitOfWork.CommitAsync();
            }
            await _unitOfWork.CommitAsync();
            var sessionDetailId = sessionDetailDeliverers.First().SessionDetailId;
            var ordersInSessionDetail = await _orderRepository.GetBySessionDetailId(sessionDetailId);
            var exchangeGiftsInSessionDetail = await _exchangeGiftRepository.GetBySessionDetailId(sessionDetailId);
            var availableDeliverers = await _delivererRepository.GetBySessionDetailId(sessionDetailId);
            foreach (var o in ordersInSessionDetail)
            {
                o.DelivererId = Guid.Empty;
            }
            foreach (var e in exchangeGiftsInSessionDetail)
            {
                e.DelivererId = Guid.Empty;
            }
            foreach (var order in ordersInSessionDetail)
            {
                AssignOrderToDelivererAsync(order, order.Profile!.User!, ordersInSessionDetail, availableDeliverers);
            }
            
            foreach (var exchangeGift in exchangeGiftsInSessionDetail)
            {
                AssignExchangeGiftToDelivererAsync(exchangeGift, exchangeGift.Profile!.User!, exchangeGiftsInSessionDetail, ordersInSessionDetail, availableDeliverers);
            }
            foreach (var exchangeGift in exchangeGiftsInSessionDetail)
            {
                exchangeGift.Profile = null;
                await _exchangeGiftRepository.UpdateAsync(exchangeGift);
                await _unitOfWork.CommitAsync();
            }
            foreach (var order in ordersInSessionDetail)
            {
                //order.Profile = null;
                await _orderRepository.UpdateAsync(order);
                await _unitOfWork.CommitAsync();
            }

        }
        //public async Task AssignOrderToDelivererAndUpdateAsync(Order order, User customer, ICollection<Order> ordersInSessionDetail, ICollection<SessionDetailDeliverer> availableDeliverers)
        //{
        //    AssignOrderToDelivererAsync(order, customer, ordersInSessionDetail, availableDeliverers);
        //    order.Profile = null;
        //    await _orderRepository.UpdateAsync(order);
        //    await _unitOfWork.CommitAsync();
        //}
        public void AssignOrderToDelivererAsync(Order order, User customer, ICollection<Order> ordersInSessionDetail, ICollection<SessionDetailDeliverer> availableDeliverers)
        {
            //var data = await _orderRepository.GetDelivererIdAndOrderCountBySessionDetailId(order.SessionDetailId);
            var data = ordersInSessionDetail.Where(o => o.DelivererId != Guid.Empty)
                .GroupBy(o => o.DelivererId)
                .Select(g => new GetDelivererIdAndOrderCountBySessionDetailIdResponse
                {
                    DelivererId = g.Key,
                    OrderCount = g.Count(),
                    CustomerIds = g.Select(o => o.Profile!.UserId).ToHashSet()
                }).ToList();
            if (availableDeliverers.IsNullOrEmpty())
            {
                throw new InvalidRequestException(MessageConstants.SessionDetailMessageConstrant.NoDelivererAvailableInThisSession);
            }
            var delivererThatAlreadyHasOrderOfThisCustomer = data.FirstOrDefault(d => d.CustomerIds.Any(id => id == customer.Id));
            if (delivererThatAlreadyHasOrderOfThisCustomer != null)
            {
                order.DelivererId = delivererThatAlreadyHasOrderOfThisCustomer.DelivererId;
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
                order.DelivererId = sortedData.First().DelivererId;
            }
        }
        public async Task<ICollection<SessionDetailDeliverer>> GetBySessionDetailId(Guid sessionDetailId)
        {
            return await _delivererRepository.GetBySessionDetailId(sessionDetailId);
        }
        
        public void AssignExchangeGiftToDelivererAsync(ExchangeGift exchangeGift, User customer, ICollection<ExchangeGift> exchangeGiftsInSessionDetail, ICollection<Order> ordersInSessionDetail, ICollection<SessionDetailDeliverer> availableDeliverers)
        {
            var data = exchangeGiftsInSessionDetail.Where(o => o.DelivererId != Guid.Empty)
                .GroupBy(o => o.DelivererId)
                .Select(g => new GetDelivererIdAndOrderCountBySessionDetailIdResponse
                {
                    DelivererId = g.Key.Value,
                    OrderCount = g.Count(),
                    CustomerIds = g.Select(o => o.Profile!.UserId).ToHashSet()
                }).ToList();
            data.AddRange(
                    ordersInSessionDetail.Where(o => o.DelivererId != Guid.Empty)
                .GroupBy(o => o.DelivererId)
                .Select(g => new GetDelivererIdAndOrderCountBySessionDetailIdResponse
                {
                    DelivererId = g.Key,
                    OrderCount = g.Count(),
                    CustomerIds = g.Select(o => o.Profile!.UserId).ToHashSet()
                }).ToList()
                );
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

        }
    }
}
