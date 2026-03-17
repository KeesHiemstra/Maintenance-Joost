using QuickLog.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public List<string> OptionList { get; set; }

    public MainWindow()
    {
      VM = new MainViewModel(this);

      InitializeComponent();

      #region Window Title

      string name = Assembly.GetExecutingAssembly().GetName().Name;
      string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
#if DEBUG
      Title = $"{name} - {version} (Debug)";
#else
      Title = $"{name} - {version}";
#endif

      #endregion

      #region Initialize the options for the combo box
      OptionList = new List<string>();
      OptionList.Add("");
      OptionList.Add("Calender");
      OptionList.Add("BedTime");
      OptionComboBox.ItemsSource = OptionList;
      #endregion

      DataContext = VM;

      #region Handle command line arguments or option in the window
      VM.Option = MainViewModel.Options.None;
      string[] args = Environment.GetCommandLineArgs();
      if (args.Length > 1)
      {
        ProcessCommandLineArgs(args);
      }
      VM.ProcessOptionSelection();
      #endregion

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
      VM.QuickLogPath = args[1]; // Path to the log file

      if (args.Length > 2)
      {
        if (args[2].ToLower() == "/calender")
        {
          OptionComboBox.SelectedItem = "Calender";
          VM.Option = MainViewModel.Options.Calender;
        }
        if (args[2].ToLower() == "/bedtime")
        {
          OptionComboBox.SelectedItem = "BedTime";
          VM.Option = MainViewModel.Options.BedTime;
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

    private void OptionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if ((string)OptionComboBox.SelectedItem == "Calendar")
      {
        VM.Option = MainViewModel.Options.Calender;
      }
      else if ((string)OptionComboBox.SelectedItem == "Bedtime")
      {
        VM.Option = MainViewModel.Options.BedTime;
      }
      else
      {
        VM.Option = MainViewModel.Options.None;
      }
      VM.ProcessOptionSelection();
    }
  }
}
