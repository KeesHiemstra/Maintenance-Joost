using MaintenanceJournal.ViewModels;

using System.Windows;

namespace MaintenanceJournal.Views
{
	/// <summary>
	/// Interaction logic for CoffeeUsageWindow.xaml
	/// </summary>
	public partial class CoffeeUsageWindow : Window
	{
		public CoffeeUsageWindow(CoffeeUsageViewModel coffeeUsageViewModel)
		{
			InitializeComponent();
		}
	}
}
