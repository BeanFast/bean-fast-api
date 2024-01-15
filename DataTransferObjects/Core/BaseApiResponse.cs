using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Core
{
    public class BaseApiResponse
    {
        public DateTime Time { get; set; } = DateTime.UtcNow;

        public string Code { get; set; }

        public string Message { get; set; }
    }
}
