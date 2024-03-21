using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Settings
{
    public class TwilioSettings
    {
        public string FromNumber { get; set; } = default!;
        public string AccountSid { get; set; } = default!;
        public string BodyTemplate { get; set; } = default!;
        public string AuthToken { get; set; } = default!;
        public int OtpLifeTimeInMinutes { get; set; } = default!;
    }
}
