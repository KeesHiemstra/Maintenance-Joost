using System;
using System.Globalization;

namespace MaintenanceJournal.ViewModels
{
	public static class DateInfo
	{
		static readonly DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
		static readonly Calendar cal = dfi.Calendar;

		public static int DateTimeWeekNumber(DateTime date)
		{
			int week = cal.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek,
				DayOfWeek.Monday);

			if (week == 52 && date.Month == 1) { }
			else if (week == 53)
			{
				if (date.Month == 12 && date.DayOfWeek <= DayOfWeek.Wednesday)
				{
					week = 1;
				}
				else if (date.Month == 1 && date.DayOfWeek >= DayOfWeek.Thursday) { }
			}

			return week;
		}

	}
}
