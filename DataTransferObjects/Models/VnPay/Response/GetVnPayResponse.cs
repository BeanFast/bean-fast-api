using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.VnPay.Response
{
    public class GetVnPayResponse
    {
        public bool Success { get; set; }
        public string PaymentMethod { get; set; }
        public string WalletDescription { get; set; }
        public string WalletId { get; set; }
        public string PaymentId { get; set; }
        public string TransactionId { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }
    }
}
