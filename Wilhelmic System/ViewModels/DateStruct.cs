using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Reactive.Linq;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace Wilhelmic_System.ViewModels
{
    public struct DateStruct : IComparable<DateStruct>, IEquatable<DateStruct>
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public DateStruct(int year, int month, int day)
        {
            if (month < 0 || month > 13) throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 0 and 13.");
            if (day < 1 || day > GetDaysInMonth(year, month)) throw new ArgumentOutOfRangeException(nameof(day), $"Day must be between 1 and {GetDaysInMonth(year, month)} for month {month}.");

            Year = year;
            Month = month;
            Day = day;
        }

        private static int GetDaysInMonth(int year, int month)
        {
            if (month == 0) return IsLeapYear(year) ? 2 : 1; // return that month 0 is 2 days long if Leap Year, otherwise 1 day long.
            return 28; // Months 1-13 have a fixed 28 days
        }

        private static bool IsLeapYear(int year)
        {
            // Leap year calculation
            return (year % 4 == 0 && year % 100 != 0) || year % 400 == 0;
        }

        public int CompareTo(DateStruct other)
		{
			if (Year != other.Year)
				return Year.CompareTo(other.Year);
			if (Month != other.Month)
				return Month.CompareTo(other.Month);
			return Day.CompareTo(other.Day);
		}

		public bool Equals(DateStruct other)
		{
			return Year == other.Year && Month == other.Month && Day == other.Day;
		}

		public override bool Equals(object obj)
		{
			if (obj is DateStruct)
			{
				return Equals((DateStruct)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Year, Month, Day);
		}
		public static bool operator ==(DateStruct left, DateStruct right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(DateStruct left, DateStruct right)
		{
			return !(left == right);
		}
	}
}