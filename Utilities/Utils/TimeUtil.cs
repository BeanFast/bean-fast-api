using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Utils
{
    public class TimeUtil
    {
        public static int ConvertTimeToSeconds(string timeStr)
        {
            if (string.IsNullOrEmpty(timeStr))
            {
                throw new ArgumentNullException(nameof(timeStr));
            }

            // Split the string into hours, minutes, and seconds
            var parts = timeStr.Split(':');

            if (parts.Length != 3)
            {
                throw new ArgumentException("Invalid time format. Expected HH:MM:SS.", nameof(timeStr));
            }

            // Parse individual parts into integers
            int hours = int.Parse(parts[0]);
            int minutes = int.Parse(parts[1]);
            int seconds = int.Parse(parts[2]);

            // Validate input (optional)
            if (hours < 0 || minutes < 0 || seconds < 0 || seconds >= 60)
            {
                throw new ArgumentOutOfRangeException(nameof(timeStr), "Time values cannot be negative or have more than 60 seconds.");
            }

            // Calculate total seconds
            return hours * 3600 + minutes * 60 + seconds;
        }
        public static DateTime GetCurrentVietNamTime()
        {
            return DateTime.UtcNow.AddHours(7);
        }
    }
}
