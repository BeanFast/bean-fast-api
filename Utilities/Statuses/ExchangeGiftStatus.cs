using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Statuses
{
    public class ExchangeGiftStatus : BaseEntityStatus
    {

        public static readonly int Delivering = 4;
        public static readonly int Completed = 5;
        public static readonly int CancelledByCustomer = 6;
        public static readonly int Cancelled = 7;
    }
}
