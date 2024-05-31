using Microsoft.Extensions.Options;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Settings;

namespace Services.Implements
{
    public class EsmsSmsService : ISmsService
    {
        private AppSettings _appSettings;

        public EsmsSmsService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public async Task SendSmsAsync(string toPhone, string message)
        {
            // Create a GET request
            var httpClient = new HttpClient();
            var template = "la ma xac minh dang ky Baotrixemay cua ban";
            var url = $"http://rest.esms.vn/MainService.svc/json/SendMultipleMessage_V4_get?Phone={toPhone}&Content={message + " " + template}&ApiKey={_appSettings.Esms.ApiKey}&SecretKey={_appSettings.Esms.SecretKey}&Brandname={_appSettings.Esms.BrandName}&SmsType=2";
            var response = await httpClient.GetAsync(url);
            await Console.Out.WriteLineAsync(await response.Content.ReadAsStringAsync());
            if (!response.IsSuccessStatusCode)
            {
                await Console.Out.WriteLineAsync(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
