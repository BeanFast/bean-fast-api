using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Utilities.Settings;

namespace Services.Implements
{
    public class SmsService : ISmsService
    {
        
        private AppSettings _appSettings;

        public SmsService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task SendSmsAsync(string toPhone, string message)
        {
            TwilioClient.Init(_appSettings.Twilio.AccountSid, _appSettings.Twilio.AuthToken);
            var smsMessage = await MessageResource.CreateAsync(
                    from: new Twilio.Types.PhoneNumber(_appSettings.Twilio.FromNumber),
                    to: new Twilio.Types.PhoneNumber(toPhone),
                    body: message
                );
        }
    }
}
