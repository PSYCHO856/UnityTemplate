using System;

namespace Watermelon
{
    public static class TimeUtils
    {
        private static readonly DateTime DEFAULT_DATE_UNIX = new(1970, 1, 1, 0, 0, 0);

        /// <summary>
        ///     Get current Unix time
        /// </summary>
        public static double GetCurrentUnixTimestamp()
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0);

            var unixTimestamp = (DateTime.Now - date).TotalSeconds;

            return unixTimestamp;
        }

        /// <summary>
        ///     Get current Unix time
        /// </summary>
        public static double GetCurrentUnixTimestampWithExtraTime(int weeks = 0, int days = 0, int hours = 0,
            int minutes = 0, int seconds = 0)
        {
            var nowDate = DateTime.Now;
            nowDate = nowDate.AddDays(weeks * 7);
            nowDate = nowDate.AddDays(days);
            nowDate = nowDate.AddHours(hours);
            nowDate = nowDate.AddMinutes(minutes);
            nowDate = nowDate.AddSeconds(seconds);

            var unixTimestamp = (nowDate - DEFAULT_DATE_UNIX).TotalSeconds;

            return unixTimestamp;
        }

        /// <summary>
        ///     Get current Unix time
        /// </summary>
        public static double GetCurrentDayUnixTimestamp()
        {
            var nowDate = DateTime.Now;
            nowDate = nowDate.AddHours(-nowDate.Hour);
            nowDate = nowDate.AddMinutes(-nowDate.Minute);
            nowDate = nowDate.AddSeconds(-nowDate.Second);

            var unixTimestamp = (nowDate - DEFAULT_DATE_UNIX).TotalSeconds;

            return unixTimestamp;
        }

        /// <summary>
        ///     Convert DateTime to Unix time
        /// </summary>
        public static double GetUnixTimestampFromDateTime(DateTime target)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);

            var unixTimestamp = (target - date).TotalSeconds;

            return unixTimestamp;
        }

        /// <summary>
        ///     Convert Unix time to DateTime
        /// </summary>
        public static DateTime GetDateTimeFromUnixTime(double unixTime)
        {
            return DEFAULT_DATE_UNIX.AddSeconds(unixTime);
        }
    }
}