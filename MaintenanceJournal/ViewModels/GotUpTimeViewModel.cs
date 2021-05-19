
using MaintenanceJournal.Models;
using MaintenanceJournal.Views;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MaintenanceJournal.ViewModels
{
	public class GotUpTimeViewModel
	{
		#region [ Fields ]

		private readonly MainViewModel VM;
		private GotUpTimeReportWindow View;
		private GotUpTimeGraphWindow Graph;
		private List<(DateTime Date, TimeSpan Time)> _Journals;

		#endregion

		#region [ Properties ]

		public GotUpTime Overall { get; set; } = new GotUpTime();
		public GotUpTime WorkWeek { get; set; } = new GotUpTime();
		public GotUpTime Weekend { get; set; } = new GotUpTime();
		public List<GotUpTime> Months { get; set; } = new List<GotUpTime>();
		public Dictionary<int, int> TimeCount = new Dictionary<int, int>();

		#endregion

		#region [ Constructor ]

		public GotUpTimeViewModel(MainViewModel mainViewModel)
		{
			VM = mainViewModel;
		}

		#endregion

		#region [ Public methods ]

		public void ShowReport()
		{
			GotUpTimeReportWindow view = new GotUpTimeReportWindow(this)
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

		public void ShowGraph()
		{
			GotUpTimeGraphWindow graph = new GotUpTimeGraphWindow(this)
			{
				Left = VM.View.Left + 100,
				Top = VM.View.Top + 20,
			};
			Graph = graph;
			Graph.DataContext = this;

			//Collect the window data
			CollectData();
			CollectGraphData();
			Graph.BuildGraph();
			View.ShowDialog();
		}

		#endregion

		/// <summary>
		/// The data is used for both report and graph.
		/// </summary>
		private void CollectData()
		{
			_Journals = VM.Journals
				.Where(x => x.Event == "Opgestaan")
				.Select(x => (x.DTStart.Value.Date, x.DTStart.Value.TimeOfDay))
				.ToList();
		}

		#region Report

		/// <summary>
		/// The data for report.
		/// </summary>
		private void CollectReportData()
		{
			Overall = CollectPeriode(_Journals);
			WorkWeek = CollectWeek();
			Weekend = CollectWeekEnd();
			CollectMonths();
		}

		private GotUpTime CollectWeek()
		{
			List<(DateTime Date, TimeSpan Time)> journal = _Journals
				.Where(x => x.Date.DayOfWeek >= DayOfWeek.Monday && x.Date.DayOfWeek <= DayOfWeek.Friday)
				.ToList();
			return CollectPeriode(journal);
		}

		private GotUpTime CollectWeekEnd()
		{
			List<(DateTime Date, TimeSpan Time)> journal = _Journals
				.Where(x => x.Date.DayOfWeek < DayOfWeek.Monday || x.Date.DayOfWeek > DayOfWeek.Friday)
				.ToList();
			return CollectPeriode(journal);
		}

		private void CollectMonths()
		{
			DateTime date = DateTime.Now;
			date = new DateTime(date.Year, date.Month, 1).AddMonths(1);

			while (date > _Journals.Min(x => x.Date))
			{
				GotUpTime gotUpTime = new GotUpTime();
				gotUpTime = CollectPeriode(_Journals
					.Where(x => x.Date < date && x.Date >= date.AddMonths(-1))
					.ToList());
				gotUpTime.Period = $"{date.AddMonths(-1):yyyy-MMM}";

				Months.Add(gotUpTime);
				date = date.AddMonths(-1);
			}
		}

		/// <summary>
		/// Collect the date and time from Journals.
		/// </summary>
		/// <param name="_journals"></param>
		/// <returns></returns>
		private GotUpTime CollectPeriode(List<(DateTime Date, TimeSpan Time)> _journals)
		{
			GotUpTime result = new GotUpTime()
			{
				Min = _journals.Min(x => x.Time),
				Max = _journals.Max(x => x.Time),
				Avg = TimeSpan.FromSeconds(Math.Round(_journals.Average(x => (double)x.Time.TotalSeconds)))
			};

			return result;
		}

		#endregion

		#region Graph

		private void CollectGraphData()
		{
			int MinBlock = TimeBlock(_Journals.Min(x => x.Time));
			foreach (var item in _Journals)
			{
				int block = TimeBlock(item.Time) - MinBlock;
				if (TimeCount.ContainsKey(block))
				{
					TimeCount[block]++;
				}
				else
				{
					TimeCount.Add(block, 1);
				}
			}
		}

		private int TimeBlock(TimeSpan time) 
		{
			return (int)time.TotalMinutes / 5;
		}


		#endregion

	}
}
