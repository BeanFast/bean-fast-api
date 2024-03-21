using BusinessObjects.Models;
using DataTransferObjects.Models.SmsOtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISmsOtpService
    {
        Task<SmsOtp> SendOtpAsync(User user);
        Task<bool> VerifyOtpAsync(SmsOtpVerificationRequest request, User user);
    }
}
