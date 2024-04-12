using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Statuses
{
    public class SessionStatus : BaseEntityStatus
    {
        public static readonly int Incoming = 2;
        public static readonly int Ended = 3;

    }
}
