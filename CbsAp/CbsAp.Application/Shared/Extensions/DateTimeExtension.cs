using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Shared.Extensions
{
    public static class DateTimeExtension
    {
        //Accurate conversion using real UTC → AUD conversion (default behavior)
        public static DateTime ToAudDate(this DateTimeOffset dto)
        {
            var audTimeZone = GetAudTimeZone();
            var audLocalTime = TimeZoneInfo.ConvertTimeFromUtc(dto.UtcDateTime, audTimeZone);
            return audLocalTime.Date;
        }

        //  Treat the time as if it was already in AUD local time (ignores offset)
        public static DateTime ToAudDateTreatAsLocal(this DateTimeOffset dto)
        {
            var audTimeZone = GetAudTimeZone();

            // Reconstruct the original DateTime WITHOUT applying the offset
            var localDateTime = new DateTime(dto.Year, dto.Month, dto.Day, dto.Hour, dto.Minute, dto.Second, DateTimeKind.Unspecified);

            // Interpret that DateTime as AUD local time (no conversion from UTC)
            var audDateTime = TimeZoneInfo.ConvertTime(localDateTime, audTimeZone);
            return audDateTime.Date;
        }

        // Cross-platform time zone resolver
        private static TimeZoneInfo GetAudTimeZone()
        {
            try
            {
                // Windows
                return TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                // Linux/macOS
                return TimeZoneInfo.FindSystemTimeZoneById("Australia/Sydney");
            }
        }
    }
}
