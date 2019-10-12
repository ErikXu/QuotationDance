using System;

namespace QuotationDance.Collector.Parse
{
    public class TimeParse
    {
        public static DateTime Parse(string func, string str)
        {
            switch (func)
            {
                case "ParseUnixMilliseconds":
                    return ParseUnixMilliseconds(str);
                case "ParseUnixSeconds":
                    return ParseUnixSeconds(str);
                default:
                    return ParseUtc(str);
            }
        }

        private static DateTime ParseUnixMilliseconds(string str)
        {
            var millisecond = long.Parse(str);
            var offset = DateTimeOffset.FromUnixTimeMilliseconds(millisecond);
            return offset.UtcDateTime;
        }

        private static DateTime ParseUnixSeconds(string str)
        {
            var second = long.Parse(str);
            var offset = DateTimeOffset.FromUnixTimeSeconds(second);
            return offset.UtcDateTime;
        }

        private static DateTime ParseUtc(string str)
        {
            return DateTime.Parse(str);
        }
    }
}