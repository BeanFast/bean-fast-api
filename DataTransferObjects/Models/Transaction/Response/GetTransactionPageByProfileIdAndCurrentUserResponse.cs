using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Transaction.Response
{
    public class GetTransactionPageByProfileIdAndCurrentUserResponse
    {
        public Guid? Id { get; set; }
        public OrderOfGetTransactionPageByProfileIdAndCurrentUserResponse? Order { get; set; }
        public ExchangeGiftOfGetTransactionPageByProfileIdAndCurrentUserResponse ExchangeGift { get; set; }
        public GameOfGetTransactionPageByProfileIdAndCurrentUserResponse Game { get; set; }
        public Guid? WalletId { get; set; }
        public string Code { get; set; }
        public DateTime Time { get; set; }
        public double Value { get; set; }
        public int Status { get; set; }

        public class OrderOfGetTransactionPageByProfileIdAndCurrentUserResponse
        {
            public Guid Id { get; set; }

            public string Code { get; set; }
        }

        public class ExchangeGiftOfGetTransactionPageByProfileIdAndCurrentUserResponse
        {
            public Guid Id { get; set; }

            public string Code { get; set; }
        }

        public class GameOfGetTransactionPageByProfileIdAndCurrentUserResponse
        {
            public Guid Id { get; set; }

            public string Code { get; set; }

            public string Name { get; set; }
        }
    }
}
