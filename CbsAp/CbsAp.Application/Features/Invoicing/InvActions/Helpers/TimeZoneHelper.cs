using System;



namespace CbsAp.Application.Shared.Helpers
{
    public static class TimeZoneHelper
    {
        private const string PhilippineTimeZoneId = "Singapore Standard Time";



        private static TimeZoneInfo? _phTimeZone;



        public static TimeZoneInfo PhilippineTimeZone =>
        _phTimeZone ??= TimeZoneInfo.FindSystemTimeZoneById(PhilippineTimeZoneId);



        /// <summary>
                /// Convert DateTimeOffset? to Philippine Time (UTC+8)
                /// </summary>
        public static DateTimeOffset? ToPhilippineTime(this DateTimeOffset? date)
        {
            if (!date.HasValue)
                return null;



            return TimeZoneInfo.ConvertTime(date.Value, PhilippineTimeZone);
        }



        /// <summary>
                /// Convert DateTimeOffset to Philippine Time (UTC+8)
                /// </summary>
        public static DateTimeOffset ToPhilippineTime(this DateTimeOffset date)
        {
            return TimeZoneInfo.ConvertTime(date, PhilippineTimeZone);
        }
    }
}