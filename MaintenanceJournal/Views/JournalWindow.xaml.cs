using MaintenanceJournal.ViewModels;

using System.Windows;
using System.Windows.Input;

namespace MaintenanceJournal.Views
{
	/// <summary>
	/// Interaction logic for JournalWindow.xaml
	/// </summary>
	public partial class JournalWindow : Window
	{
		readonly JournalViewModel JournalVM;

		public JournalWindow(JournalViewModel journalVM)
		{
			InitializeComponent();

			JournalVM = journalVM;
			DataContext = journalVM;
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			JournalVM.SaveRecord();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			JournalVM.CancelRecord();
		}

		private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this.Close();
		}

		private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !JournalVM.IsNewRecord;
		}

		private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			JournalVM.DeleteRecord();
		}

	}
}
