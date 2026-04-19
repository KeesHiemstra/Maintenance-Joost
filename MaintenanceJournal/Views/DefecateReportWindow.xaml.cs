using MaintenanceJournal.ViewModels;

using System.Windows;

namespace MaintenanceJournal.Views
{
	/// <summary>
	/// Interaction logic for DefecateReportWindow.xaml
	/// </summary>
	public partial class DefecateReportWindow : Window
	{
		internal DefecateReportWindow(DefecateViewModel defecateViewModel)
		{
			InitializeComponent();
		}

		private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Escape)
			{
				Close();
			}
		}

	}
}

