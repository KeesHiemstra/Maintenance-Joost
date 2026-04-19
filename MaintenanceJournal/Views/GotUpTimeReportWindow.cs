using MaintenanceJournal.ViewModels;

using System.Windows;
using System.Windows.Input;

namespace MaintenanceJournal.Views
{
	/// <summary>
	/// Interaction logic for GotUpTimeReportWindow.xaml
	/// </summary>
	public partial class GotUpTimeReportWindow : Window
	{
		public GotUpTimeReportWindow(GotUpTimeViewModel gotUpTimeViewModel)
		{
			InitializeComponent();
		}

		private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				Close();
			}
		}

	}
}
