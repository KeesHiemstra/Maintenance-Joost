using MaintenanceJournal.Models;
using MaintenanceJournal.Views;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceJournal.ViewModels
{
	internal class UrineViewModel
	{
		#region [ Fields ]

		private readonly MainViewModel VM;
		private UrineReportWindow View;

		#endregion

		#region [ Properties ]

		public List<UrineDay> UrineDays { get; set; }
		public List<UrineReport> UrineWeeks { get; set; }
		public List<UrineReport> UrineMonths { get; set; }

		#endregion

		#region [ Constructor ]

		public UrineViewModel(MainViewModel mainViewModel)
		{
			VM = mainViewModel;
		}

		#endregion

		#region [ Public methods ]

		public void ShowReport()
		{
			UrineReportWindow view = new UrineReportWindow(this)
			{
				Left = VM.View.Left + 100,
				Top = VM.View.Top + 20,
			};
			View = view;
			View.DataContext = this;

			//Collect the window data
			CollectData();
			CollectReportData();
			View.ShowDialog();
		}

		#endregion

		private void CollectData()
		{
			List<(DateTime Date, string Message)> _Journal;

			_Journal = VM.Journals
				.Where(x => x.Event == "Kleine boodschap")
				.Select(x => (x.DTStart.Value, x.Message))
				.ToList();

			UrineDays = new List<UrineDay>();

			foreach (var (Date, Message) in _Journal)
			{
				UrineDay day = new UrineDay
				{
					Date = Date
				};

				string[] raw = Message.Split(' ');
				day.Total = byte.Parse(raw[0]);
				day.Night = byte.Parse(raw[1]);
				if (raw.Length > 2)
				{
					day.Info = raw[2];
				}
				UrineDays.Add(day);
			}
		}

		private void CollectReportData()
		{
			CollectReportDataWeek();
			CollectReportDataMonth();
		}

		private void CollectReportDataWeek()
		{
			UrineWeeks = new List<UrineReport>();

			DateTime startDate = UrineDays.Min(x => x.Date.Date);
			startDate = startDate.AddDays(-((int)startDate.DayOfWeek) + 1);

			DateTime endDate = UrineDays.Max(x => x.Date.Date);
			while (startDate < endDate)
			{
				UrineReport urineReport = new UrineReport()
				{
					Period = WeekNumber(startDate),
				};

				var days = UrineDays
					.Where(x => x.Date >= startDate && x.Date < startDate.AddDays(7))
					.ToList();
				if (days.Count() == 0)
				{
					startDate = startDate.AddDays(7);
					continue;
				}

				urineReport.TotalMin = days.Min(x => x.Total);
				urineReport.TotalAvg = days.Average(x => x.Total);
				urineReport.TotalMax = days.Max(x => x.Total);
				urineReport.NightMin = days.Min(x => x.Night);
				urineReport.NightAvg = days.Average(x => x.Night);
				urineReport.NightMax = days.Max(x => x.Night);

				UrineWeeks.Insert(0, urineReport);

				startDate = startDate.AddDays(7);
			}
		}

		private void CollectReportDataMonth()
		{
			UrineMonths = new List<UrineReport>();

			DateTime startDate = UrineDays.Min(x => x.Date.Date);
			startDate = startDate.AddDays(-(startDate.Day) + 1);

			DateTime endDate = UrineDays.Max(x => x.Date.Date);
			while (startDate < endDate)
			{
				UrineReport urineReport = new UrineReport()
				{
					Period = $"{startDate:yyyy-MMM}",
				};

				var days = UrineDays
					.Where(x => x.Date >= startDate && x.Date < startDate.AddMonths(1))
					.ToList();
				if (days.Count() == 0)
				{
					startDate = startDate.AddMonths(1);
					continue;
				}

				urineReport.TotalMin = days.Min(x => x.Total);
				urineReport.TotalAvg = days.Average(x => x.Total);
				urineReport.TotalMax = days.Max(x => x.Total);
				urineReport.NightMin = days.Min(x => x.Night);
				urineReport.NightAvg = days.Average(x => x.Night);
				urineReport.NightMax = days.Max(x => x.Night);

				UrineMonths.Insert(0, urineReport);

				startDate = startDate.AddMonths(1);
			}
		}

		private string WeekNumber(DateTime date)
		{
			DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
			System.Globalization.Calendar cal = dfi.Calendar;

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
