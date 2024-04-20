using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.User.Response
{
    public class GenerateQrCodeResponse
    {
        public string QrCode { get; set; }

        public DateTime QrCodeExpiry { get; set; }
    }
}
