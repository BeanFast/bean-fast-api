using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Constants
{
    public class TimeConstrant
    {
        public static readonly int NumberOfMinutesBeforeDeliveryStartTime = 60;
        public static readonly int DeliveryStartHour = 4;
        public static readonly int DeliveryEndHour = 11;
        public const int GapBetweenOrderEndTimeAndDeliveryStartTimeInMinutes = 360;
    }
}
