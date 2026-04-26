using CHi.Extensions;

using Joost;

using MaintenanceJournal.Models;
using MaintenanceJournal.Views;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MaintenanceJournal.ViewModels
{
	public class EjaculationViewModel
	{

		#region [ Fields ]

		private readonly MainViewModel VM;
		private EjaculationWindow View;

		#endregion

		#region [ Properties ]

		public List<Ejaculation> EjaculationList { get; set; }

		#endregion

		#region [ Construction ]

		public EjaculationViewModel(MainViewModel mainVM)
		{
				VM = mainVM;
		}

		#endregion

		#region [ Public methods ]

		public void ShowReport()
		{
			EjaculationWindow view = new EjaculationWindow(this)
			{
				Left = VM.View.Left + 100,
				Top = VM.View.Top + 20,
			};
			View = view;
			CollectReport();
			View.Show();
		}

		#endregion

		private void CollectReport()
		{

			List <Journal> ejaculations = VM.Journals
				.Where(x => x.Event == "Zaadlozing")
				.OrderBy(x => x.DTStart)
				.Select(x => x)
				.ToList();

			CollectReportDay(ejaculations);
			CollectReportWeek();
			CollectReportMonth();
			CollectReportQuarter();
		}

		private void CollectReportDay(List<Journal> ejaculations)
		{
			View.EjaculationDaysDataGrid.ItemsSource = null;

			EjaculationList = new List<Ejaculation>();

			foreach (Journal journal in ejaculations)
			{
				int count = int.Parse(journal.Message);
				EjaculationList.Insert(0, new Ejaculation
				{
					Date = journal.DTStart.Value,
					Total = count
				});
			}

			View.EjaculationDaysDataGrid.ItemsSource = EjaculationList;
		}

		private void CollectReportWeek()
		{
			View.EjaculationWeeksDataGrid.ItemsSource = null;

			List<EjaculationReport> ejaculationList = new List<EjaculationReport>();

			DateTime startDate = EjaculationList.Min(x => x.Date).WeekStart();
			DateTime endDate = EjaculationList.Max(x => x.Date);

			while (startDate <= endDate)
			{
				DateTime weekEnd = startDate.AddDays(7);
				int total = EjaculationList
					.Where(x => x.Date >= startDate && x.Date < weekEnd)
					.Sum(x => x.Total);
				ejaculationList.Insert(0, new EjaculationReport	
				{
					Date = startDate.WeekNumber(),
					Total = total
				});
				startDate = startDate.AddDays(7);
			}

			View.EjaculationWeeksDataGrid.ItemsSource = ejaculationList;
		}

		private void CollectReportMonth()
		{
			View.EjaculationMonthsDataGrid.ItemsSource = null;

			List<EjaculationReport> ejaculationList = new List<EjaculationReport>();

			DateTime startDate = EjaculationList.Min(x => x.Date).MonthStart();
			DateTime endDate = EjaculationList.Max(x => x.Date);

			while (startDate <= endDate)
			{
				DateTime monthEnd = startDate.AddMonths(1).AddDays(-1);
				int total = EjaculationList
					.Where(x => x.Date >= startDate && x.Date <= monthEnd)
					.Sum(x => x.Total);
				ejaculationList.Insert(0, new EjaculationReport
				{
					Date = startDate.ToString("yyyy-MMM"),
					Total = total
				});
				startDate = startDate.AddMonths(1);
			}

			View.EjaculationMonthsDataGrid.ItemsSource = ejaculationList;
		}

		private void CollectReportQuarter()
		{
			View.EjaculationQuartersDataGrid.ItemsSource = null;

			List<EjaculationReport> ejaculationList = new List<EjaculationReport>();

			DateTime startDate = EjaculationList.Min(x => x.Date).QuarterStart();
			DateTime endDate = EjaculationList.Max(x => x.Date);

			while (startDate <= endDate)
			{
				DateTime quarterEnd = startDate.AddMonths(3).AddDays(-1);
				int total = EjaculationList
					.Where(x => x.Date >= startDate && x.Date <= quarterEnd)
					.Sum(x => x.Total);
				ejaculationList.Insert(0, new EjaculationReport
				{
					Date = startDate.QuarterNumber(),
					Total = total
				});
				startDate = startDate.AddMonths(3);
			}

			View.EjaculationQuartersDataGrid.ItemsSource = ejaculationList;
		}

	}
}
