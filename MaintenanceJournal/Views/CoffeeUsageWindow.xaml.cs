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

		private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Escape)
			{
				Close();
			}
		}

	}
}