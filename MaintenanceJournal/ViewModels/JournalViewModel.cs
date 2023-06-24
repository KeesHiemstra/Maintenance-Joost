using CHi.Log;

using Joost;

using MaintenanceJournal.Models;
using MaintenanceJournal.Views;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MaintenanceJournal.ViewModels
{
	public class JournalViewModel
	{

		#region [ Fields ]

		private readonly MainViewModel VM;
		private JournalWindow View;

		#endregion

		#region [ Properties ]

		public Journal Record { get; set; }
		public List<string> Events { get; set; }
		public bool IsNewRecord;

		#endregion

		#region [ Construction ]

		public JournalViewModel(MainViewModel mainVM)
		{
			VM = mainVM;
			Events = new List<string>(VM.Journals
				.Select(x => x.Event)
				.Distinct()
				.OrderBy(x => x)
				.ToList());
		}

		#endregion

		#region [ Public methods ]

		public void ShowJournal(int? logID)
		{
			if (logID == null)
			{
				//New record
				IsNewRecord = true;
				Record = new Journal()
				{
					DTStart = DateTime.Now,
					DTCreation = DateTime.Now,
				};
				Log.Write("Edit new journal record");
			}
			else
			{
				//Existing record
				//Record = (from j in VM.Db.Journals
				//					where j.LogID == logID
				//					select j).SingleOrDefault();
				Record = VM.Journals
					.Where(x => x.LogID == logID)
					.FirstOrDefault();
				Log.Write($"Edit journal record: {logID}");
			}

			JournalWindow view = new JournalWindow(this)
			{

			};
			View = view;

			View.EventComboBox.ItemsSource = Events;
			View.MessageTextBox.Focus();
			View.Show();
		}

		#endregion

		internal void SaveRecord()
		{
			if (IsNewRecord)
			{
				VM.Db.Journals.Add(Record);
			}

			if (string.IsNullOrWhiteSpace(Record.Event))
			{
				Record.Event = null;
			}

			VM.Db.SaveChanges();
			View.Close();
			Log.Write($"Saved journal record: {Record.LogID.ToString() ?? "<new>"}");
			//Added this line to enable updates the MainDataGrid
			VM.View.FilterMessageTextBox.Focus();
			if (IsNewRecord) { VM.GetJournals(IsNewRecord); }
		}

		internal void CancelRecord()
		{
			Log.Write($"Canceled journal record: {Record.LogID.ToString() ?? "<new>"}");
			Record = null;
			View.Close();
		}

		internal void DeleteRecord()
		{
			if (MessageBox.Show($"Delete record with ID {Record.LogID.ToString() ?? "<new>"}?",
				"Delete",
				MessageBoxButton.YesNoCancel,
				MessageBoxImage.Question) != MessageBoxResult.Yes) { return; };

			var record = VM.Db.Journals.Find(Record.LogID);
			if (record != null)
			{
				Log.Write($"Delete journal record: {Record.LogID.ToString() ?? "<new>"}");
				VM.Db.Journals.Remove(record);
				VM.Db.SaveChanges();
			}

			View.Close();

			//Reopen the restored database and refresh view
			VM.View.MainDataGrid.ItemsSource = null;
			VM.GetJournals();
			VM.View.MainDataGrid.ItemsSource = VM.Journals;
		}
	}
}
