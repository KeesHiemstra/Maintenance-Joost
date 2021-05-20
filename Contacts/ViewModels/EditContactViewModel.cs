using Joost;

using Contacts.Views;

using System;
using System.Linq;

namespace Contacts.ViewModels
{
	public class EditContactViewModel
	{
		#region [ Fields ]

		private readonly MainViewModel VM;
		private EditContactWindow View;
		private bool IsNewRecord;

		#endregion

		#region [ Properties ]

		public Journal Contact { get; set; }
		public DateTime Date { get; set; }
		public string Time { get; set; }

		#endregion

		#region [ Construction ]

		public EditContactViewModel(MainViewModel mainVM)
		{
			VM = mainVM;
		}

		#endregion

		#region [ Public methods ]

		public void ShowContact(Journal contact)
		{
			if (contact == null)
			{
				//New record
				IsNewRecord = true;
				Contact = new Journal()
				{
					Event = "Contact",
					DTStart = DateTime.Now,
					DTCreation = DateTime.Now,
				};
			}
			else
			{
				Contact = contact;
			}

			Date = Contact.DTStart.Value.Date;
			Time = Contact.DTStart.Value.ToString("HH:mm");

			EditContactWindow view = new EditContactWindow(this);
			View = view;

			View.MessageTextBox.Focus();
			View.Show();
		}

		#endregion

		internal void SaveContact()
		{
			if (IsNewRecord)
			{
				VM.Contacts.Add(Contact);
			}

			if (string.IsNullOrWhiteSpace(Contact.Event))
			{
				Contact.Event = null;
			}

			//Save the date and time to DTStart
			TimeSpan.TryParse(Time, out TimeSpan time);
			Contact.DTStart = Date.Date.AddMinutes(time.TotalMinutes);

			VM.ContactsIsChanged = true;
			View.Close();
		}
		
		internal void CancelContact()
		{
			Contact = null;
			View.Close();
		}
	}
}
