﻿using QuickLog.ViewModels;

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuickLog
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainViewModel VM { get; set; }

		public MainWindow()
		{
			VM = new MainViewModel(this);

			InitializeComponent();

			DataContext = VM;

			string[] args = Environment.GetCommandLineArgs();
			if (args.Length > 1)
			{
				ProcessCommandLineArgs(args);
			}

			//if (LogsDataGrid.ItemsSource != null)
			{
				LogsDataGrid.ItemsSource = VM.Logs
					.OrderByDescending(x => x.Time);
			}

			EventComboBox.ItemsSource = VM.Items;

			TimeTextBox.Focus();
		}

		private void ProcessCommandLineArgs(string[] args)
		{
			foreach (string item in args)
			{
				if (item.ToLower() == "/calender")
				{
					VM.SetCalendarOnly();
				}
				else
				{
					VM.QuickLogPath = item;
				}
			}
		}

		#region Menu MainCommands

		#region Exit command
		private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void ExitCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		#endregion

		#region Rename command
		private void RenameCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void RenameCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			VM.Rename();
		}

		#endregion

		#region Save command
		private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void SaveCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			VM.SaveLogs();
		}

		#endregion

		#region Clear command
		private void ClearCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void ClearCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			VM.ClearLogs();
		}

		#endregion

		#endregion

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			//if (VM.Logs != null && VM.Logs.Count > 0)
			//{
			//	DateDatePicker.SelectedDate = VM
			//		.Logs.OrderByDescending(x => x.Time)
			//		.Last().Time.Date;
			//	EventComboBox.Text = VM.Logs.Last().Event;
			//}

			VM.MainViewModel_Loaded();
		}

		private void TimeTextBox_KeyUp(object sender, KeyEventArgs e)
		{
			if (sender == null) { return; }

			if (e.Key is Key.OemPlus or Key.Space or Key.Add)
			{
				DateDatePicker.SelectedDate = DateDatePicker.SelectedDate.Value.Date.AddDays(1);
				DateDatePicker.DisplayDate = DateDatePicker.SelectedDate.Value;
			}
			else if (e.Key is Key.OemMinus or Key.Subtract)
			{
				DateDatePicker.SelectedDate = DateDatePicker.SelectedDate.Value.Date.AddDays(-1);
				DateDatePicker.DisplayDate = DateDatePicker.SelectedDate.Value;
			}

			return;
		}

		private void TimeTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			//Prevent action if is not focused
			if (!((TextBox)e.Source).IsFocused) { return; }

			//Prevent errors
			string text = ((TextBox)e.Source).Text;
			if (string.IsNullOrEmpty(text)) { return; }

			//Keep the caret position
			int caretIndex = ((TextBox)e.Source).CaretIndex;

			//Prevent longer time text
			if (text.Length > 4)
			{
				((TextBox)e.Source).Text = text.Substring(0, 4);
				((TextBox)e.Source).CaretIndex = caretIndex;
				return;
			}

			//Allow only numbers
			foreach (char check in text)
			{
				if (check is < '0' or > '9')
				{
					((TextBox)e.Source).Text = text.Replace(check.ToString(), "");
					((TextBox)e.Source).CaretIndex = caretIndex;
					return;
				}
			}
		}

		private void TimeTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			string text = ((TextBox)e.Source).Text;
			//Prevent errors
			if (string.IsNullOrEmpty(text)) { return; }
			//Prevent adding a colon if it already exist
			if (text.Contains(":")) { return; }

			//Add zeros and a colon
			text = text.PadLeft(4, '0');
			((TextBox)e.Source).Text = text.Insert(text.Length - 2, ":");
		}

		private void MessageTextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			((TextBox)e.OriginalSource).SelectAll();
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			VM.AddRecord();
		}

		/// <summary>
		/// Auto save the logs.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//I filled 2 times the logs. It costs a lot, but I didn't use the auto save.
			VM.SaveLogs();
		}

		private void CalendarOnlyCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			VM.SetCalendarOnly();
		}

		private void CalendarOnlyCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			VM.ClearCalendarOnly();
		}
	}
}
