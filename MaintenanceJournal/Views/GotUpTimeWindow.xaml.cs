using MaintenanceJournal.ViewModels;

using System.Windows;

namespace MaintenanceJournal.Views
{
	/// <summary>
	/// Interaction logic for GotUpTimeWindow.xaml
	/// </summary>
	public partial class GotUpTimeWindow : Window
	{
		public GotUpTimeWindow(GotUpTimeViewModel gotUpTimeViewModel)
		{
			InitializeComponent();
		}
	}
}
