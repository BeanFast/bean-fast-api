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
using Utilities.Exceptions;
using Utilities.Settings;

namespace Services.Implements
{
    public class ExchangeGiftService : BaseService<ExchangeGift>, IExchangeGIftService
    {

        private readonly IGiftService _giftService;
        private readonly IProfileService _profileService;
        private readonly ISessionDetailService _sessionDetailService;
        public ExchangeGiftService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, IGiftService giftService, IProfileService profileService, ISessionDetailService sessionDetailService) : base(unitOfWork, mapper, appSettings)
        {
            _giftService = giftService;
            _profileService = profileService;
            _sessionDetailService = sessionDetailService;
        }

        public async Task CreateExchangeGiftAsync(ExchangeGiftRequest request, Guid customerId)
        {
            var gift = await _giftService.GetGiftByIdAsync(request.GiftId);
            var profile = await _profileService.GetProfileByIdAndCurrentCustomerIdAsync(request.ProfileId, customerId);
            var sessionDetail = await _sessionDetailService.GetByIdAsync(request.SessionDetailId);
            if (sessionDetail.Session!.OrderEndTime.CompareTo(DateTime.Now) >= 0)
            {
                throw new ClosedSessionException();
            }
            if (sessionDetail.Location!.SchoolId != profile.SchoolId)
            {
                throw new InvalidSchoolException();
            }
            var exchangeGift = _mapper.Map<ExchangeGift>(request);
            //exchangeGift.Status = ExchangeGiftStatus.Pending;
            exchangeGift.Points = gift.Points;
            exchangeGift.PaymentDate = DateTime.Now;
            //exchangeGift.
            exchangeGift.Transactions = new List<Transaction>
            {
                new Transaction
                {
                    Value = -gift.Points,
                    Time = DateTime.Now,
                    WalletId = profile.Wallets.FirstOrDefault(w => w.Type == "Ví điểm").Id,
                    Code = "ExchangeGift",
                    ExchangeGiftId = exchangeGift.Id
                }
            };
            await Console.Out.WriteLineAsync(sessionDetail.ToString());
        }

    }
}
