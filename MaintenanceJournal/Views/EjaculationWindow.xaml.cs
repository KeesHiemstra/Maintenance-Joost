using MaintenanceJournal.ViewModels;

using System.Windows;

namespace MaintenanceJournal.Views
{
	/// <summary>
	/// Interaction logic for EjaculationWindow.xaml
	/// </summary>
	public partial class EjaculationWindow : Window
	{

		public EjaculationWindow(EjaculationViewModel ejaculationModelView)
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
