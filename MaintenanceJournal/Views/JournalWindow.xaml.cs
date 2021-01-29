using MaintenanceJournal.ViewModels;

using System.Windows;

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
	}
}
