using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;
using Utilities.Settings;

namespace Services.Implements
{
    public class SmsService : ISmsService
    {
        private readonly AppSettings _appSettings;
        public SmsService(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task SendSmsAsync(string phone, string message)
        {
            var smsMessage = await MessageResource.CreateAsync(
                from: new Twilio.Types.PhoneNumber(_appSettings.Twilio.FromNumber),
                to: new Twilio.Types.PhoneNumber(phone),
                body: _appSettings.Twilio.BodyTemplate + message
                );
        }
    }
}
