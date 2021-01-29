using CHi.Extensions;
using CHi.Log;

using MaintenanceJournal.Views;

using Newtonsoft.Json;

using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace MaintenanceJournal.ViewModels
{
	public class OptionsViewModel : INotifyPropertyChanged
	{

		#region [ Fields ]

		private string dbConnection = @"Database=Joost_Dev;Data Source=(Local);Trusted_Connection=True;MultipleActiveResultSets=true";
		private string backupPath = @"C:\";
		private string restoreFile = string.Empty;
		private bool databaseChanged = false;

		#endregion

		#region [ Properties ]

		public string DbConnection
		{
			get => dbConnection;
			set
			{
				if (value != dbConnection)
				{
					dbConnection = value;
					databaseChanged = true;
					NotifyPropertyChanged();
				}
			}
		}

		public string BackupPath
		{
			get => backupPath;
			set
			{
				if (value != backupPath)
				{
					backupPath = value.SavePath();
					NotifyPropertyChanged();
				}
			}
		}

		public string RestoreFile
		{
			get => restoreFile;
			set
			{
				if (value != restoreFile)
				{
					restoreFile = value;
					NotifyPropertyChanged();
				}
			}
		}

		[JsonIgnore]
		public string DbName
		{
			get
			{
				string result = string.Empty;
				string[] parts = DbConnection.Split(';');
				for (int i = 0; i < parts.Length; i++)
				{
					if (parts[i].ToLower().StartsWith("database"))
					{
						string[] option = parts[i].Split('=');
						result = option[1];
					}
				}
				return result;
			}
		}

		[JsonIgnore]
		public string JsonPath => Assembly.GetEntryAssembly().Location.Replace(".exe", ".json");

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region [ Construction ]


		#endregion

		#region [ Public methods ]

		public void ShowOptions(MainWindow main)
		{
			OptionsWindow view = new OptionsWindow(this)
			{
				Top = main.Top + 30,
				Left = main.Left + 50
			};

			bool? Result = view.ShowDialog();
			if ((bool)Result)
			{
				string json = JsonConvert.SerializeObject(this, Formatting.Indented);
				using StreamWriter stream = new StreamWriter(JsonPath);
				stream.Write(json);
				Log.Write($"Options are save to '{JsonPath}'");
				if (databaseChanged)
				{
					main.UpdateDatabaseConnection();
				}
			}
		}

		#endregion

		private void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
