using Contacts.ViewModels;

using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Contacts
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region [ Fields ]

		private readonly MainViewModel VM;

		#endregion

		#region [ Construction ]

		public MainWindow()
		{
			InitializeComponent();

			string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			Title = $"Contacts ({version})";

			VM = new MainViewModel();
			DataContext = VM;
		}

		#endregion

		#region [ Menu methods ]

		#region Exit command

		private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void ExitCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		#endregion

		#region ClearContacts command

		private void ClearContactsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = VM.Contacts.Count > 0;
		}

		private void ClearContactsCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			VM.ClearContacts();
		}

		#endregion

		#region Export command

		private void ExportCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = VM.Contacts.Count > 0;
		}

		private void ExportCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			VM.ExportToJoost();
		}

		#endregion

		#region NewContact command

		private void NewContactCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void NewContactCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			VM.EditContact(null);
		}

		#endregion

		#endregion

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (VM.ContactsIsChanged)
			{
				VM.SaveContacts();
			}
		}

		private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			VM.DoubleClickDataGrid(sender, e);
		}
	}
}
