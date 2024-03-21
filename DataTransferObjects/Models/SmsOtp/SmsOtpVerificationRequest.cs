using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.SmsOtp
{
    public class SmsOtpVerificationRequest
    {
        public string Phone { get; set; }
        public string OtpValue { get; set; }
    }
}
