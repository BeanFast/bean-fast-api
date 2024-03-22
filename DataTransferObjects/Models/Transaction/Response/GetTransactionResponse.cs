using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Transaction.Response
{
    public class GetTransactionResponse
    {
        public Guid? OrderId { get; set; }
        public Guid? ExchangeGiftId { get; set; }
        public Guid WalletId { get; set; }
        public string Code { get; set; }
        public DateTime Time { get; set; }
        public double Value { get; set; }
    }
}
