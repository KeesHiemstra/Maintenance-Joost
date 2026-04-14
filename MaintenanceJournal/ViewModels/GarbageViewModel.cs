using Joost;

using MaintenanceJournal.Models;
using MaintenanceJournal.Views;

using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace MaintenanceJournal.ViewModels
{
	public class GarbageViewModel
	{

		#region [ Fields ]

		private readonly MainViewModel VM;
		private GarbageWindow View;

		#endregion

		#region [ Properties ]

		#endregion

		#region [ Construction ]

		public GarbageViewModel(MainViewModel mainVM)
		{
			VM = mainVM;
		}

		#endregion

		#region [ Public methods ]

		public void ShowReport()
		{
			GarbageWindow view = new GarbageWindow(this)
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
			List<Journal> garbage = VM.Journals
				.Where(x => x.Event == "Afval")
				.OrderBy(x => x.DTStart)
				.Select(x => x)
				.ToList();


			CollectList(garbage.Where(x => x.Message
													.ToLower().Contains("gft")).ToList(), View.GftReportDataGrid);
			CollectList(garbage.Where(x => x.Message
													.ToLower().Contains("glas")).ToList(), View.GlassReportDataGrid);
			CollectList(garbage.Where(x => x.Message
													.ToLower().Contains("papier")).ToList(), View.PaperReportDataGrid);
			CollectList(garbage.Where(x => x.Message
													.ToLower().Contains("plastic")).ToList(), View.PlasticReportDataGrid);
			CollectList(garbage.Where(x => x.Message
													.ToLower().Contains("rest")).ToList(), View.RestReportDataGrid);

		}

		private void CollectList(List<Journal> garbage, DataGrid dataGrid)
		{
			List<Garbage> report = new List<Garbage>();
			dataGrid.ItemsSource = null;

			int count = 0;
			for (int i = 0; i < garbage.Count; i++)
			{
				Garbage item = new Garbage
				{
					Date = garbage[i].DTStart.Value,
					Count = count++,
					Weeks = (i == 0) ? 0 : 
						(int)((garbage[i].DTStart.Value - garbage[i - 1].DTStart.Value).TotalDays + 4) / 7
				};
				report.Insert(0, item);
			}

			dataGrid.ItemsSource = report;
		}

	}
}
