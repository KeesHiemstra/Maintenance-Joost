using MaintenanceJournal.ViewModels;

using System.Windows;
using System.Windows.Input;

namespace MaintenanceJournal.Views
{
	/// <summary>
	/// Interaction logic for UrineReportWindow.xaml
	/// </summary>
	public partial class UrineReportWindow : Window
	{
		internal UrineReportWindow(UrineViewModel urineViewModel)
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
