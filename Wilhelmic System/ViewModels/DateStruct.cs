using System;

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
				return Year.CompareTo(other.Year); // if the year isn't the same as the other year, return it to be decided whether bigger or smaller
			if (Month != other.Month)
				return Month.CompareTo(other.Month); // if the month isn't the same as the other year, return it to be decided whether bigger or smaller
            return Day.CompareTo(other.Day); // return the day to be decided whether bigger or smaller
		}

		public bool Equals(DateStruct other)
		{
			return Year == other.Year && Month == other.Month && Day == other.Day; // determine whether year, month, and day are equal to other year, month, day
		}

		public override bool Equals(object obj) // overrides Equals, in case object being compared is potentially not a DateStruct
		{
			if (obj is DateStruct)
			{
				return Equals((DateStruct)obj); // if it is a DateStruct report true, that it is a DateStruct
			}
			return false; // if it is not a DateStruct return false.
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Year, Month, Day); // return full date as a single hash in order to be used in dictionary if necessary.
		}
		public static bool operator ==(DateStruct left, DateStruct right) 
		{
			return left.Equals(right); // returns that left is the same as right
		}

		public static bool operator !=(DateStruct left, DateStruct right)
		{
			return !(left == right); // returns that the right and left are not the same
		}
	}
}