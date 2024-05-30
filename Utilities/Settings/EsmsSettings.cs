using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Settings
{
    public class EsmsSettings
    {
        public string ApiKey { get; set; } = default!;
        public string SecretKey { get; set; } = default!;
        public string BrandName { get; set; } = default!;
    }
}
