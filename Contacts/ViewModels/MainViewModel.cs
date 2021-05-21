
using Joost;

using Newtonsoft.Json;

using System.Collections.Generic;
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
		private static readonly string EventsPath = "\\\\Rommeldijk\\Data\\JoostEvents.json";

		#endregion

		#region [ Properties ]

		public ObservableCollection<Journal> Contacts { get; set; } = 
			new ObservableCollection<Journal>();
		public List<string> Events { get; set; } = new List<string>();
		public bool ContactsIsChanged { get; set; }

		#endregion

		#region [ Construction ]

		public MainViewModel()
		{
			LoadContacts();
			LoadEvents();
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

		private void LoadEvents()
		{
			if (!File.Exists(EventsPath)) return;

			using (StreamReader stream = File.OpenText(EventsPath))
			{
				string json = stream.ReadToEnd();
				Events = JsonConvert.DeserializeObject<List<string>>(json);
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

		public void SaveEvents()
		{
			string json = JsonConvert.SerializeObject(Events, Formatting.Indented);
			using (StreamWriter stream = new StreamWriter(EventsPath))
			{
				stream.Write(json);
			}
		}

		internal void ExportToJoost()
		{
			_ = new ProcessContactsToJoost(this);
		}

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
