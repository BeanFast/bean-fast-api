using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
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
using Utilities.Exceptions;
using Utilities.Settings;
using Utilities.Statuses;

namespace Services.Implements
{
    public class LoyaltyCardService : BaseService<LoyaltyCard>, ILoyaltyCardService
    {
        public LoyaltyCardService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper, appSettings)
        {
        }
        public async Task<bool> CheckLoyaltyCardWithQRCode(string qrCode)
        {
            if(string.IsNullOrEmpty(qrCode))
            {
                throw new InvalidRequestException(MessageConstants.LoyaltyCardMessageConstrant.QRCodeNotFound);
            }

            List<Expression<Func<LoyaltyCard, bool>>> filters = new()
            {
                (loyaltyCard) => qrCode.Equals(loyaltyCard.QRCode)
            };
            var loyaltyCard = await _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active, filters: filters);

            if (loyaltyCard != null)
            {
                return true;
            }

            return false;
        }

        public Task<LoyaltyCard> GetLoyaltyCardByQRCode(string qrCode)
        {
            List<Expression<Func<LoyaltyCard, bool>>> filters = new()
            {
                (loyaltyCard) => qrCode.Equals(loyaltyCard.QRCode)
            };
            var loyaltyCard = _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active, filters: filters,
                include: queryable => queryable.Include(lc => lc.Profile!))
                ?? throw new EntityNotFoundException(MessageConstants.LoyaltyCardMessageConstrant.InvalidLoyaltyCard);
            return loyaltyCard!;
        }
    }
}
