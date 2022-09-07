using MaintenanceJournal.Models;
using MaintenanceJournal.Views;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace MaintenanceJournal.ViewModels
{
	internal class DefecateViewModel
	{
		#region [ Fields ]

		private readonly MainViewModel VM;
		private DefecateReportWindow View;

		#endregion

		#region [ Properties ]

		public List<DefecateDetail> DefecateDetails { get; set; }
		public List<DefecateReport> DefecateDays { get; set; }
		public List<DefecateReport> DefecateWeeks { get; set; }
		public List<DefecateReport> DefecateMonths { get; set; }

		#endregion

		#region [ Constructor ]

		public DefecateViewModel(MainViewModel mainViewModel)
		{
			VM = mainViewModel;
		}

		#endregion

		#region [ Public methods ]

		public void ShowReport()
		{
			DefecateReportWindow view = new DefecateReportWindow(this)
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
				.Where(x => x.Event == "Grote boodschap")
				.Select(x => (x.DTStart.Value, x.Message))
				.ToList();

			DefecateDetails = new List<DefecateDetail>();

			foreach (var (Date, Message) in _Journal)
			{
				DefecateDetail day = new DefecateDetail
				{
					Time = Date
				};

				string[] raw = Message.Split(',');
				day.Wips = byte.Parse(raw[0]);
				if (raw.Length > 1)
				{
					day.Info = raw[1].Trim();
				}
				DefecateDetails.Add(day);
			}
		}

		private void CollectReportData()
		{
			CollectReportDataDay();
			CollectReportDataWeek();
			CollectReportDataMonth();
		}

		private void CollectReportDataDay()
		{
			DefecateDays = new List<DefecateReport>();

			DateTime startDate = DefecateDetails.Min(x => x.Time.Date);
			//startDate = startDate.AddDays(-((int)startDate.DayOfWeek) + 1);

			DateTime endDate = DefecateDetails.Max(x => x.Time.Date).AddDays(1);
			while (startDate < endDate)
			{
				DefecateReport report = new DefecateReport()
				{
					Date = startDate.Date,
					Period = startDate.Date.ToString("yyyy-MM-dd ddd"),
				};

				var days = DefecateDetails
					.Where(x => x.Time >= startDate && x.Time < startDate.AddDays(1) &&
								 x.Wips > 0)
					.ToList();
				if (days.Count() == 0)
				{
					startDate = startDate.AddDays(1);
					continue;
				}

				report.TotalGoings = (byte)days.Count();
				report.TotalWips = (byte)days.Sum(x => x.Wips);
				report.MinWips = days.Min(x => x.Wips);
				report.AvgWips = days.Average(x => x.Wips);
				report.MaxWips = days.Max(x => x.Wips);

				DefecateDays.Insert(0, report);

				startDate = startDate.AddDays(1);
			}
		}

		private void CollectReportDataWeek()
		{
			DefecateWeeks = new List<DefecateReport>();

			DateTime startDate = DefecateDays.Min(x => x.Date);
			startDate = startDate.AddDays(-((int)startDate.DayOfWeek) + 1);

			DateTime endDate = DefecateDays.Max(x => x.Date.Date).AddDays(7);
			while (startDate < endDate)
			{
				DefecateReport report = new DefecateReport()
				{
					Period = WeekNumber(startDate),
				};

				var days = DefecateDays
					.Where(x => x.Date >= startDate && x.Date < startDate.AddDays(7) &&
								 x.TotalWips > 0)
					.ToList();
				if (days.Count() == 0)
				{
					startDate = startDate.AddDays(7);
					continue;
				}

				report.TotalGoings = days.Sum(x => x.TotalGoings);
				report.MinGoings = (byte)days.Min(x => x.TotalGoings);
				report.AvgGoings = days.Average(x => x.TotalGoings);
				report.MaxGoings = (byte)days.Max(x => x.TotalGoings);
				report.TotalWips = days.Sum(x => x.TotalWips);
				report.MinWips = (byte)days.Min(x => x.MinWips);
				report.AvgWips = days.Average(x => x.AvgWips);
				report.MaxWips = (byte)days.Max(x => x.MaxWips);

				DefecateWeeks.Insert(0, report);

				startDate = startDate.AddDays(7);
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

		private void CollectReportDataMonth()
		{
			DefecateMonths = new List<DefecateReport>();

			DateTime startDate = DefecateDays.Min(x => x.Date);
			startDate = startDate.AddDays(-(startDate.Day) + 1);

			DateTime endDate = DefecateDays.Max(x => x.Date.Date);
			while (startDate < endDate)
			{
				DefecateReport report = new DefecateReport()
				{
					Period = $"{startDate:yyyy-MMM}",
				};

				var days = DefecateDays
					.Where(x => x.Date >= startDate && x.Date < startDate.AddMonths(1) &&
								 x.TotalWips > 0)
					.ToList();
				if (days.Count() == 0)
				{
					startDate = startDate.AddMonths(1);
					continue;
				}

				report.TotalGoings = days.Sum(x => x.TotalGoings);
				report.MinGoings = (byte)days.Min(x => x.TotalGoings);
				report.AvgGoings = days.Average(x => x.TotalGoings);
				report.MaxGoings = (byte)days.Max(x => x.TotalGoings);
				report.TotalWips = days.Sum(x => x.TotalWips);
				report.MinWips = (byte)days.Min(x => x.MinWips);
				report.AvgWips = days.Average(x => x.AvgWips);
				report.MaxWips = (byte)days.Max(x => x.MaxWips);

				DefecateMonths.Insert(0, report);

				startDate = startDate.AddMonths(1);
			}
		}

	}
}
