using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Transaction.Response
{
    public class GetTransactionsForDashBoardResponse
    {
        public int Value { get; set; }
        public string Month { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
