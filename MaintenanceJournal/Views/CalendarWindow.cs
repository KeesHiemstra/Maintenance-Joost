using MaintenanceJournal.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MaintenanceJournal.Views
{
	/// <summary>
	/// Interaction logic for CalenderWindow.xaml
	/// </summary>
	public partial class CalendarWindow : Window
	{

		#region [ Fields ]

		private readonly CalendarViewModel VM;

		#endregion

		#region [ Properties ]


		#endregion

		#region [ Construction ]

		public CalendarWindow(CalendarViewModel viewModel)
		{
			InitializeComponent();

			VM = viewModel;
			FilterFromDatePicker.DisplayDateEnd = DateTime.Now;
			FilterToDatePicker.DisplayDateEnd = DateTime.Now;
		}

		#endregion

		#region [ Public methods ]


		#endregion

		private void FilterButton_Click(object sender, RoutedEventArgs e)
		{
			VM.ApplyFilter();
		}
	}
}
