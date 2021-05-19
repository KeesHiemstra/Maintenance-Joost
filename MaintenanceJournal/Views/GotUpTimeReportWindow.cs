using MaintenanceJournal.ViewModels;

using System.Windows;

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
	}
}
