using CsvHelper;
using CsvHelper.Configuration;

using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		
		public enum Options
		{
			None,
			Calender,
			BedTime,
			AfternoonEvent
		};
		public Options Option { get; set; } = Options.None;
		public ObservableCollection<QLog> Logs { get; set; } = new ObservableCollection<QLog>();
		public List<string> Items { get; set; } = new List<string>();
		public string QuickLogPath = "\\\\Rommeldijk\\Data\\QuickLog.csv";

		public MainViewModel(MainWindow view)
		{
			View = view;
		}

		/// <summary>
		/// Set the date from the first log entry.
		/// </summary>
		internal void MainViewModel_Loaded()
		{
			LoadLogs();

			if (Logs.Count > 0)
			{
				View.DateDatePicker.SelectedDate = Logs.First().Time.Date;
				if (Option != Options.None)
				{
					DateNext();
				}
			}

			UpdateItems();
			ToFocus();
		}

		internal void ProcessOptionSelection()
		{
			switch (Option)
			{
				case Options.None:
					View.OptionComboBox.SelectedItem = 0;
					View.TimeTextBox.IsEnabled = true;
					View.MessageTextBox.IsEnabled = true;
					break;
				case Options.Calender:
					View.OptionComboBox.SelectedItem = 1;
					View.TimeTextBox.IsEnabled = false;
					View.MessageTextBox.IsEnabled = true;
					break;
				case Options.BedTime:
					View.OptionComboBox.SelectedIndex = 2;
					View.TimeTextBox.IsEnabled = true;
					View.MessageTextBox.IsEnabled = false;
					break;
				case Options.AfternoonEvent:
					View.OptionComboBox.SelectedItem = 3;
					View.TimeTextBox.IsEnabled = false;
					View.TimeTextBox.Text = "1200";
					View.MessageTextBox.IsEnabled = true;
					break;
			}
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
			}

			View.LogsDataGrid.ItemsSource = Logs;
		}

		/// <summary>
		/// Add a log entry to QuickLog data.
		/// </summary>
		internal void AddRecord()
		{
			DateTime time;

			if (Option == Options.AfternoonEvent)
			{
				time = DateTime.Parse($"{View.DateDatePicker.SelectedDate.Value.Date:yyyy-MM-dd} 12:00");
				View.TimeTextBox.Text = "12:00";
				View.MessageTextBox.Focus();
			}
			else
			{
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
			}

			View.LogsDataGrid.ItemsSource = null;

			Logs.Insert(0, new QLog()
			{
				Time = time,
				Message = View.MessageTextBox.Text,
				Event = View.EventComboBox.Text
			});

			UpdateItems();

			switch (Option)
			{
				case Options.None:
					View.DateDatePicker.SelectedDate = time.Date;
					View.TimeTextBox.Text = "";
					break;
				case Options.Calender:
					DateNext();
					break;
				case Options.BedTime:
					DateNext();
					View.TimeTextBox.Text = "";
					break;
				case Options.AfternoonEvent:
					DateNext();
					View.TimeTextBox.Text = "12:00";
					break;
			}

			ToFocus();

			View.LogsDataGrid.ItemsSource = Logs
				.OrderByDescending(x => x.Time);
		}

		/// <summary>
		/// Focus to the correct control based on the selected option.
		/// </summary>
		internal void ToFocus()
		{
			switch (Option)
			{
				case Options.None:
					View.TimeTextBox.Focus();
					break;
				case Options.Calender:
					View.MessageTextBox.Focus();
					break;
				case Options.BedTime:
					View.TimeTextBox.Focus();
					break;
				case Options.AfternoonEvent:
					View.MessageTextBox.Focus();
					break;
			}
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
			Logs = new ObservableCollection<QLog>(logs);
		}

		/// <summary>
		/// Save the QuickLog data.
		/// </summary>
		internal void SaveLogs()
		{
			if (Logs == null || Logs.Count == 0)
				return;

			if (!File.Exists(QuickLogPath))
			{
				FileStream fs = null;
				try
				{
					fs = new FileStream(QuickLogPath, FileMode.CreateNew);
				}
				finally
				{
					fs?.Dispose();
				}
			}
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

		public void DateNext(bool ByKey = false)
		{
			if (View.DateDatePicker.SelectedDate == null)
				return;

			if (ByKey)
			{
				View.DateDatePicker.SelectedDate = View.DateDatePicker.SelectedDate.Value.Date.AddDays(1);
				View.DateDatePicker.DisplayDate = View.DateDatePicker.SelectedDate.Value;
			}
			else
			{
				View.DateDatePicker.SelectedDate = Logs.First().Time.Date.AddDays(1);
			}
		}

		public void DatePrevious(bool ByKey = false)
		{
			if (View.DateDatePicker.SelectedDate == null)
				return;

			if (ByKey)
			{
				View.DateDatePicker.SelectedDate = View.DateDatePicker.SelectedDate.Value.Date.AddDays(-1);
				View.DateDatePicker.DisplayDate = View.DateDatePicker.SelectedDate.Value;
			}
			else
			{
				View.DateDatePicker.SelectedDate = Logs.First().Time.Date.AddDays(-1);
			}
		}

	}
}
