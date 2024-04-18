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
			Year = year;
			Month = month;
			Day = day;
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
			return HashCode.Combine(Year, Month, Day)
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