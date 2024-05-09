using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.SmsOtp;
using Repositories.Interfaces;
using Utilities.Utils;

namespace Repositories.Implements;

public class SmsRepository : GenericRepository<SmsOtp>, ISmsRepository
{
    public SmsRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {
    }
    public async Task<bool> VerifyOtpAsync(SmsOtpVerificationRequest request, User user)
    {
        var otp = await FirstOrDefaultAsync(filters: new()
            {
                otp => otp.Value == request.OtpValue,
                otp => otp.UserId == user.Id,
            }, orderBy: o => o.OrderByDescending(otp => otp.CreateAt));
        if (otp == null || otp.ExpiredAt < TimeUtil.GetCurrentVietNamTime()) return false;
        return true;
    }
}