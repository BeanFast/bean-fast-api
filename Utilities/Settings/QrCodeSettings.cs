using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Settings
{
    public class QrCodeSettings
    {
        public string QrCodeSecretKey { get; set; } = default!;

        public int QrCodeExpiryInSeconds { get; set; } = default!;
    }
}
