using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implements
{
    public class EsmsSmsService : ISmsService
    {
        public async Task SendSmsAsync(string toPhone, string message)
        {
            // Create a GET request
            var httpClient = new HttpClient();
            var template = "la ma xac minh dang ky Baotrixemay cua ban";
            var url = $"http://rest.esms.vn/MainService.svc/json/SendMultipleMessage_V4_get?Phone={toPhone}&Content={message + " " + template}&ApiKey=FAF6FD3C283DA55C7440CE65D24C52&SecretKey=6FF1A369ECB792225BF6FFB0379457&Brandname=Baotrixemay&SmsType=2";
            var response = await httpClient.GetAsync(url);
            if(!response.IsSuccessStatusCode)
            {
                await Console.Out.WriteLineAsync(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
