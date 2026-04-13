using System;
using System.Globalization;

namespace CHi.Extensions
{
	public static class DateExtention
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

	}
}
