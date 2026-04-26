using System;
using System.Globalization;

namespace CHi.Extensions
{
	public static class DateExtentions
	{
		/// <summary>
		/// Returns the week number of the year for the given date in the format "YYYY-wWW", 
		/// where YYYY is the year and WW is the week number. 
		/// The week starts on Monday and the first week of the year is the one that contains at 
		/// least four days of the new year. This method also handles edge cases for weeks that 
		/// span across years.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string WeekNumber(this DateTime date)
		{
			DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
			Calendar cal = dfi.Calendar;

			int year = date.Year;
			int week = cal.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

			if (week == 52 && date.Month == 1)
			{
				year--;
			}
			else if (week == 53)
			{
				if (date.Month == 12 && date.DayOfWeek <= DayOfWeek.Wednesday)
				{
					week = 1;
					year++;
				}
				else if (date.Month == 1 && date.DayOfWeek >= DayOfWeek.Thursday)
				{
					year--;
				}
			}

			return $"{year}-w{week:00}";
		}

		/// <summary>
		/// Returns a string representing the calendar quarter and year of the specified date.
		/// </summary>
		/// <param name="date">The date for which to determine the quarter and year.</param>
		/// <returns>A string in the format "yyyy-qN", where "yyyy" is the year and "N" is the 
		/// quarter number (1 to 4) corresponding to the specified date.</returns>
		public static string QuarterNumber(this DateTime date)
		{
			int quarter = (date.Month - 1) / 3 + 1;
			return $"{date.Year}-q{quarter}";
		}

		/// <summary>
		/// Returns a new DateTime representing the start of the week for the specified date, 
		/// using Monday as the first day of the week.
		/// </summary>
		/// <remarks>This method treats Monday as the first day of the week, following the 
		/// ISO 8601 standard. The returned DateTime has its time component set to 00:00:00.</remarks>
		/// <param name="date">The date for which to determine the start of the week.</param>
		/// <returns>A DateTime value set to the date of the Monday of the same week as the 
		/// specified date, with the time component set to midnight.</returns>
		public static DateTime WeekStart(this DateTime date)
		{
			int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
			return date.AddDays(-diff).Date;
		}

		/// <summary>
		/// Returns a new DateTime representing the first day of the month for the specified date.
		/// </summary>
		/// <param name="date">The date for which to retrieve the first day of its month.</param>
		/// <returns>A DateTime set to the first day of the month and the same time component as 
		/// midnight for the specified date.</returns>
		public static DateTime MonthStart(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1);
		}

		/// <summary>
		/// Returns a new DateTime representing the first day of the quarter in which the 
		/// specified date occurs.
		/// </summary>
		/// <param name="date">The date for which to determine the start of the quarter.</param>
		/// <returns>A DateTime set to the first day of the quarter containing the specified date,
		/// with the same year and time set to midnight.</returns>
		public static DateTime QuarterStart(this DateTime date)
		{
			int quarter = (date.Month - 1) / 3 + 1;
			int startMonth = (quarter - 1) * 3 + 1;
			return new DateTime(date.Year, startMonth, 1);
		}

		/// <summary>
		/// Return written past time since now.
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public static string ShowAge(this DateTime time)
		{
			string result = string.Empty;
			DateTime now = DateTime.Now;
			TimeSpan diff = now - time;
			DateTime calcDate = time;
			int years = 0;

			if (time > now)
				return "In the future";

			// Years
			if (diff.Days >= 365)
			{
				years = (int)(diff.Days / 365.25);
				diff = diff.Subtract(TimeSpan.FromDays(years * 365.25));
				calcDate = time.AddYears(years);
				result = $"{years} year{(years != 1 ? "s" : "")} ";
			}

			// Months
			int months = 0;
			while (calcDate.AddMonths(1) <= now)
			{
				months++;
				calcDate = calcDate.AddMonths(1);
			}
			if (months == 12)
			{
				years++;
				result = $"{years} year{(years != 1 ? "s" : "")} ";
				diff = now - calcDate.AddYears(1);
				months = 0;
			}
			if (months > 0)
			{
				result += $"{months} month{(months != 1 ? "s" : "")} ";
				diff = now - calcDate;
			}

			// Days
			if (diff.Days >= 1)
			{
				result += $"{diff.Days} day{(diff.Days != 1 ? "s" : "")} ";
				diff = diff.Subtract(TimeSpan.FromDays(diff.Days));
			}

			if (diff.Hours >= 1)
			{
				result += $"{diff.Hours} hour{(diff.Hours != 1 ? "s" : "")} ";
				diff = diff.Subtract(TimeSpan.FromHours(diff.Hours));
			}

			// Hours and Minutes
			if (diff.Minutes >= 1)
			{
				result += $"{diff.Minutes} minute{(diff.Minutes != 1 ? "s" : "")}";
			}

			return result;
		}

	}
}
