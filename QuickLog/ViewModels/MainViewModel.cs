using CsvHelper;
using CsvHelper.Configuration;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace QuickLog.ViewModels
{
	public class MainViewModel
	{
		private static string InputPath = "\\\\Rommeldijk\\Data\\QuickLog.csv";
		private readonly MainWindow View;
		private readonly CsvConfiguration Config = 
			new CsvConfiguration(CultureInfo.CurrentUICulture)
		{
			Delimiter = "\t",
		};

		public List<QLog> Logs { get; set; } = new List<QLog>();
		public List<string> Items { get; set; } = new List<string>();

		public MainViewModel(MainWindow view)
		{
			View = view;

			LoadLogs();
			UpdateItems();
		}

		private void UpdateItems()
		{
			if (View.EventComboBox != null) 
			{
				View.EventComboBox.ItemsSource = null;
			}

			if (Logs != null && Logs.Count > 0)
			{
				Items = Logs.AsQueryable()
					.Select(x => x.Event)
					.OrderBy(x => x)
					.Distinct()
					.ToList();
			}

			if (View.EventComboBox != null)
			{
				View.EventComboBox.ItemsSource = Items;
				View.EventComboBox.SelectedItem = Logs.Last().Event;
			}
		}

		internal void AddRecord()
		{
			View.LogsDataGrid.ItemsSource = null;

			DateTime time = DateTime.Parse($"{View.DateDatePicker.SelectedDate.Value.Date:yyyy-MM-dd} " +
				$"{View.TimeTextBox.Text}");

			Logs.Insert(0, new QLog()
			{
				Time = time,
				Message = View.MessageTextBox.Text,
				Event = View.EventComboBox.Text
			});

			UpdateItems();

			View.TimeTextBox.Text = "";
			View.TimeTextBox.Focus();

			View.LogsDataGrid.ItemsSource = Logs
				.OrderByDescending(x => x.Time);
		}

		private void LoadLogs()
		{
			if (!File.Exists(InputPath)) { return; }
			using (var reader = new StreamReader(InputPath))
			using (var cvs = new CsvReader(reader, Config))
			{
				var logs = cvs.GetRecords<QLog>();
				Logs = new List<QLog>(logs);
			}
		}

		internal void SaveLogs()
		{
			using (var writer = new StreamWriter(InputPath))
			using (var csv = new CsvWriter(writer, Config))
			{
				csv.WriteRecords(Logs.OrderBy(x => x.Time));
			}
		}

	}
}
