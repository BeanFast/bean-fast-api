using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Statuses;

namespace Utilities.Enums
{
    public class OrderStatus : BaseEntityStatus
    {
        public static readonly int Pending = 2;
        public static readonly int Cooking = 3;
        public static readonly int Delivering = 4;
        public static readonly int Completed = 5;
        public static readonly int Cancelled = 6;
    }
}
