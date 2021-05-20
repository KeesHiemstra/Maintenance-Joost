
using Joost;

using Newtonsoft.Json;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;

namespace Contacts.ViewModels
{
	public class MainViewModel : INotifyPropertyChanged
	{
		#region [ Fields ]

		private static readonly string ContactsPath = "\\\\Rommeldijk\\Data\\Contacts.json";

		#endregion

		#region [ Properties ]

		public ObservableCollection<Journal> Contacts { get; set; } = 
			new ObservableCollection<Journal>();
		public bool ContactsIsChanged { get; set; }

		#endregion

		#region [ Construction ]

		public MainViewModel()
		{
			LoadContacts();
		}

		#endregion

		#region [ Notification ]

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		public void LoadContacts()
		{
			if (!File.Exists(ContactsPath)) return;

			using (StreamReader stream = File.OpenText(ContactsPath))
			{
				string json = stream.ReadToEnd();
				Contacts = JsonConvert.DeserializeObject <ObservableCollection<Journal>>(json);
			}
		}

		public void SaveContacts()
		{
			string json = JsonConvert.SerializeObject(Contacts, Formatting.Indented);
			using (StreamWriter stream = new StreamWriter(ContactsPath))
			{
				stream.Write(json);
			}

			ContactsIsChanged = false;
		}

		//ToDo: ExportToJoost()

		internal void EditContact(Journal contact)
		{
			EditContactViewModel vm = new EditContactViewModel(this);
			vm.ShowContact(contact);
		}

		internal void DoubleClickDataGrid(object sender, MouseButtonEventArgs e)
		{
			if (sender == null) { return; }
			foreach (Journal item in ((DataGrid)e.Source).SelectedItems)
			{
				EditContact(item);
			}
		}

		internal void ClearContacts()
		{
			Contacts.Clear();
			SaveContacts();
		}

	}
}
