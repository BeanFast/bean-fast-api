using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Transaction.Request
{
    public class TransactionFilterRequest
    {
        public string? Type { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
    }
}
