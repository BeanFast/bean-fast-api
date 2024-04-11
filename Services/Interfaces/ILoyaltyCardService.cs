using BusinessObjects.Models;
using DataTransferObjects.Models.LoyaltyCard.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ILoyaltyCardService
    {
        Task<bool> CheckLoyaltyCardWithQRCode(string qrCode);
        Task<LoyaltyCard> GetLoyaltyCardByQRCode(string qrCode);

        Task CreateLoyaltyCard(CreateLoyaltyCardRequest request, User user);
    }
}
