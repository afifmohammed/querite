using System;

namespace Querite
{
    public static class TimeHelpers
    {
        public static DateTime MinutesFrom(this int minutes, DateTime time)
        {
            return time.AddMinutes(minutes*-1);
        }

        public static TimeSpan Minutes(this int minutes)
        {
            return TimeSpan.FromMinutes(10);
        }
    }
}