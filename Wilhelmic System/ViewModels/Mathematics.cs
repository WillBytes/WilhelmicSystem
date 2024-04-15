using System;

namespace Wilhelmic_System.ViewModels
{
    public static class Mathematics
    {
        public static uint ntpYear, ntpMonth, ntpDate;
        public static byte leapYearInd, ntpHour, ntpMinute, ntpSecond;

        public static void ProcessEpoch()
        {
            DateTime currentDateTime = DateTime.UtcNow.AddHours(0);
            long epochSeconds = (long)currentDateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

            int year, month, day, hour, minute, second;
            ConvertSecondsToWilhelmic(epochSeconds, out year, out month, out day, out hour, out minute, out second);

            ntpYear = (uint)year;
            ntpMonth = (uint)month;
            ntpDate = (uint)day + 1;
            ntpHour = (byte)hour;
            ntpMinute = (byte)minute;
            ntpSecond = (byte)second;

            leapYearInd = (byte)(((year % 4 == 0 && year % 100 != 0) || year % 400 == 0) ? 1 : 0);
        }

        public static void ConvertSecondsToWilhelmic(long seconds, out int year, out int month, out int day, out int hour, out int minute, out int second)
        {
            // Constants
            const int secondsPerMinute = 60;
            const int minutesPerHour = 60;
            const int hoursPerDay = 24;
            const int daysInNormalMonth = 28;
            const int daysInFirstMonthNonLeap = 1;
            const int daysInFirstMonthLeap = 2;
            const int monthsPerYear = 14;
            const int daysPerNormalYear = daysInFirstMonthNonLeap + (monthsPerYear - 1) * daysInNormalMonth;
            const int daysPerLeapYear = daysInFirstMonthLeap + (monthsPerYear - 1) * daysInNormalMonth;

            // Convert seconds to minutes, hours, and days
            long totalSeconds = seconds;
            minute = (int)(totalSeconds / secondsPerMinute);
            totalSeconds -= minute * secondsPerMinute;
            hour = minute / minutesPerHour;
            minute -= hour * minutesPerHour;
            day = hour / hoursPerDay;
            hour -= day * hoursPerDay;
            second = (int)totalSeconds; // Any remaining seconds remain as the seconds unit

            // Calculate the year, month, and day
            year = 1970;
            while (true)
            {
                bool isLeapYear = ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0);
                int daysThisYear = isLeapYear ? daysPerLeapYear : daysPerNormalYear; //Alternate length of year

                if (day < daysThisYear) //stop the iteration if the day count is lower than the length of the year
                    break;

                day -= daysThisYear; //increase the year if day count > length of year
                year++;
            }

            // Calculate the month and the day of the month
            month = 0;
            while (true)
            {
                // Check for leap year
                bool isLeapYear = (year % 4 == 0 && year % 100 != 0) || year % 400 == 0;

                // Determine the number of days in the current month
                int daysThisMonth;
                if (month == 0) // alternate the length of the first month depending on leap or non-leap
                {
                    daysThisMonth = isLeapYear ? daysInFirstMonthLeap : daysInFirstMonthNonLeap;
                }
                else
                {
                    daysThisMonth = daysInNormalMonth;
                }

                if (day < daysThisMonth) //break the loop if day count is less than month length
                    break;

                day -= daysThisMonth; //increase month count if day count > month length
                month++;
            }
        }

        public static (int MHour, int MMinute, int MSecond) ImperialMetricConversion()
        {
            int BSecondH = (int)(ntpHour * 3600);
            int BSecondM = (int)(ntpMinute * 60);
            int BSecondS = (int)ntpSecond;
            int BSecond = BSecondH + BSecondM + BSecondS;
            int MCSecond = (int)(BSecond / 0.864);
            int MSecond = MCSecond % 100;
            MCSecond /= 100;
            int MMinute = MCSecond % 100;
            MCSecond /= 100;
            int MHour = MCSecond % 10;

            return (MHour, MMinute, MSecond);
        }

        public static string ImperialToMetricTime()
        {
            var (MHour, MMinute, MSecond) = ImperialMetricConversion();
            return $"{MHour} : {MMinute} : {MSecond}, {ntpDate}";
        }
    }
}