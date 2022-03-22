using CsvHelper;
using CsvHelper.Configuration;

using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;

namespace QuickLog.ViewModels
{
	public class MainViewModel
	{
		private readonly MainWindow View;
		private readonly CsvConfiguration Config =
			new CsvConfiguration(CultureInfo.CurrentUICulture)
			{
				Delimiter = "\t",
			};

		public List<QLog> Logs { get; set; } = new List<QLog>();
		public List<string> Items { get; set; } = new List<string>();
		public string QuickLogPath = "\\\\Rommeldijk\\Data\\QuickLog.csv";

		public MainViewModel(MainWindow view)
		{
			View = view;
		}

		internal void MainViewModel_Loaded()
		{
			LoadLogs();
			if (Logs.Count > 0)
			{
				View.DateDatePicker.SelectedDate = Logs.First().Time.Date;
			}
			UpdateItems();
		}

		internal void SetCalendarOnly()
		{
			View.CalendarOnlyCheckBox.IsChecked = true;
			View.TimeTextBox.IsEnabled = false;
			View.MessageTextBox.Focus();
		}

		internal void ClearCalendarOnly()
		{
			View.CalendarOnlyCheckBox.IsChecked = false;
			View.TimeTextBox.IsEnabled = true;
			View.TimeTextBox.Focus();
		}

		private void UpdateItems()
		{
			if (View.EventComboBox != null)
			{
				View.EventComboBox.ItemsSource = null;
			}
			View.LogsDataGrid.ItemsSource = null;

			if (Logs != null && Logs.Count > 0)
			{
				Items = Logs.AsQueryable()
					.Select(x => x.Event)
					.OrderBy(x => x)
					.Distinct()
					.ToList();
			}

			if (View.EventComboBox != null && Logs.Count > 0)
			{
				View.EventComboBox.ItemsSource = Items;
				View.EventComboBox.SelectedItem = Logs.LastOrDefault().Event;

				if (View.CalendarOnlyCheckBox.IsChecked.Value)
				{
					View.DateDatePicker.SelectedDate = Logs.FirstOrDefault().Time.AddDays(1);
				}
				else
				{
					View.DateDatePicker.SelectedDate = Logs.FirstOrDefault().Time.Date;
				}
			}

			View.LogsDataGrid.ItemsSource = Logs;
		}

		/// <summary>
		/// Add a log entry to QuickLog data.
		/// </summary>
		internal void AddRecord()
		{
			DateTime time;

			try
			{
				time = DateTime.Parse($"{View.DateDatePicker.SelectedDate.Value.Date:yyyy-MM-dd} " +
					$"{View.TimeTextBox.Text}");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message,
					"Process the entered time",
					MessageBoxButton.OK,
					MessageBoxImage.Exclamation);

				View.TimeTextBox.Focus();
				return;
			}

			View.LogsDataGrid.ItemsSource = null;

			Logs.Insert(0, new QLog()
			{
				Time = time,
				Message = View.MessageTextBox.Text,
				Event = View.EventComboBox.Text
			});

			UpdateItems();

			if (!View.CalendarOnlyCheckBox.IsChecked.Value)
			{
				View.TimeTextBox.Text = "";
				View.TimeTextBox.Focus();
			}
			else
			{
				View.MessageTextBox.Focus();
			}

			View.LogsDataGrid.ItemsSource = Logs
				.OrderByDescending(x => x.Time);
		}

		/// <summary>
		/// Clear QuickLog and save the data.
		/// </summary>
		internal void ClearLogs()
		{
			if (MessageBox.Show("Clear QuickLog data?",
				"",
				MessageBoxButton.YesNo,
				MessageBoxImage.Question,
				MessageBoxResult.No) != MessageBoxResult.Yes) { return; }

			View.LogsDataGrid.ItemsSource = null;
			Logs.Clear();
			SaveLogs();
			View.LogsDataGrid.ItemsSource = Logs;
		}

		/// <summary>
		/// Load the QuickLog data.
		/// </summary>
		private void LoadLogs()
		{
			if (!File.Exists(QuickLogPath)) { return; }
			using var reader = new StreamReader(QuickLogPath);
			using var cvs = new CsvReader(reader, Config);
			var logs = cvs.GetRecords<QLog>()
				.OrderByDescending(x => x.Time);
			Logs = new List<QLog>(logs);
		}

		/// <summary>
		/// Save the QuickLog data.
		/// </summary>
		internal void SaveLogs()
		{
			using var writer = new StreamWriter(QuickLogPath);
			using var csv = new CsvWriter(writer, Config);
			csv.WriteRecords(Logs.OrderBy(x => x.Time));
		}

		/// <summary>
		/// Rename the QuickLog.csv file.
		/// </summary>
		internal void Rename()
		{
			//Request a new name
			SaveFileDialog saveFileDialog = new SaveFileDialog()
			{
				Title = "Rename",
				DefaultExt = ".csv",
				Filter = "comma separated values (.csv)|*.csv",
				FileName = QuickLogPath,
			};

			bool? result = saveFileDialog.ShowDialog();
			if (!result.Value) { return; }
			if (saveFileDialog.FileName == QuickLogPath) { return; }

			//Rename the QuickLogPath to the new name
			try
			{
				File.Move(QuickLogPath, saveFileDialog.FileName);
				QuickLogPath = saveFileDialog.FileName;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error {saveFileDialog.FileName}:/n{ex.Message}",
					"Error renaming",
					MessageBoxButton.OK,
					MessageBoxImage.Error);
			}
		}

	}
}
