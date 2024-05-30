using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Settings;

namespace Services.Implements
{
    public class SessionDetailDelivererService : BaseService<SessionDetailDeliverer>, ISessionDetailDelivererService
    {
        private readonly ISessionDetailDelivererRepository _delivererRepository;
        private readonly IOrderService _orderService;
        private readonly IExchangeGIftService _exchangeGIftService;
        public SessionDetailDelivererService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, ISessionDetailDelivererRepository delivererRepository, IOrderService orderService, IExchangeGIftService exchangeGIftService) : base(unitOfWork, mapper, appSettings)
        {
            _delivererRepository = delivererRepository;
            _orderService = orderService;
            _exchangeGIftService = exchangeGIftService;
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
                var ordersInSessionDetail = await _orderService.GetBySessionDetailId(item.SessionDetailId);
                foreach (var order in ordersInSessionDetail)
                {
                    await _orderService.AssignOrderToDelivererAndUpdateAsync(order, order.Profile!.User!);
                }
                var exchangeGiftsInSessionDetail = await _exchangeGIftService.GetBySessionDetailId(item.SessionDetailId);
                foreach (var exchangeGift in exchangeGiftsInSessionDetail)
                {
                    await _exchangeGIftService.AssignExchangeGiftToDelivererAndUpdateAsync(exchangeGift, exchangeGift.Profile!.User!);
                }
            }
            await _unitOfWork.CommitAsync();
        }
        public async Task<ICollection<SessionDetailDeliverer>> GetBySessionDetailId(Guid sessionDetailId)
        {
            return await _delivererRepository.GetBySessionDetailId(sessionDetailId);
        }
    }
}
