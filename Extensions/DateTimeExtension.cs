using System;

namespace PFSign.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime StartOfWeek(this DateTime dateTime, DayOfWeek startDay = DayOfWeek.Monday)
        {
            int diff = (7 + (dateTime.DayOfWeek - startDay)) % 7;
            return dateTime.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime dateTime, DayOfWeek startDay = DayOfWeek.Monday)
        {
            int diff = (7 + (dateTime.DayOfWeek - startDay)) % 7;
            return dateTime.AddDays(7 - diff).Date;
        }
    }
}
