using MaintenanceJournal.ViewModels;

using System.Windows;

namespace MaintenanceJournal.Views
{
	/// <summary>
	/// Interaction logic for GarbageWindow.xaml
	/// </summary>
	public partial class GarbageWindow : Window
	{
		public GarbageWindow(GarbageViewModel garbageViewModel)
		{
			InitializeComponent();
		}

		private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Escape)
			{
				this.Close();
			}
		}

	}
}
