using BusinessObjects.Models;
using DataTransferObjects.Models.SmsOtp;

namespace Repositories.Interfaces;

public interface ISmsRepository : IGenericRepository<SmsOtp>
{
    Task<bool> VerifyOtpAsync(SmsOtpVerificationRequest request, User user);
}