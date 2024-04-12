using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.SmsOtp;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using Utilities.Settings;
using Utilities.Utils;

namespace Services.Implements
{
    public class SmsOtpService : BaseService<SmsOtp>, ISmsOtpService
    {
        private readonly ISmsService _smsService;
        public SmsOtpService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, ISmsService smsService) : base(unitOfWork, mapper, appSettings)
        {
            _smsService = smsService;
        }
        private string generateOtpValue()
        {
            Random generator = new Random();
            return generator.Next(0, 1000000).ToString("D6");
        }
        public async Task<SmsOtp> SendOtpAsync(User user)
        {
            var smsOtp = new SmsOtp();
            smsOtp.CreateAt = TimeUtil.GetCurrentVietNamTime();
            smsOtp.ExpiredAt = TimeUtil.GetCurrentVietNamTime().AddMinutes(_appSettings.Twilio.OtpLifeTimeInMinutes);  
            smsOtp.Value = generateOtpValue();
            string convertedNumber = "+84" + user.Phone;
            await _smsService.SendSmsAsync(convertedNumber, _appSettings.Twilio.BodyTemplate + smsOtp.Value);
            smsOtp.UserId = user.Id;
            await _repository.InsertAsync(smsOtp);
            await _unitOfWork.CommitAsync();
            return smsOtp;
        }
        public async Task<bool> VerifyOtpAsync(SmsOtpVerificationRequest request, User user)
        {
            var otp = await _repository.FirstOrDefaultAsync(filters: new()
            {
                otp => otp.Value == request.OtpValue,
                otp => otp.UserId == user.Id,
            }, orderBy: o => o.OrderByDescending(otp => otp.CreateAt));
            if (otp == null || otp.ExpiredAt < TimeUtil.GetCurrentVietNamTime()) return false;
            return true; 
        }
    }
}
