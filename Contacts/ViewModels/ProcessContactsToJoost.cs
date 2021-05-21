using Joost;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Contacts.ViewModels
{
	public class ProcessContactsToJoost
	{

		#region [ Fields ]

		private MainViewModel VM;
		private static string Database = "Joost_Dev";

		#endregion

		#region [ Properties ]

		public static JournalDbContext Db { get; private set; }

		#endregion

		#region [ Construction ]

		public ProcessContactsToJoost(MainViewModel mainVM)
		{
			VM = mainVM;
			ExportToJoost();
		}

		#endregion

		#region [ Public methods ]


		#endregion

		private void ExportToJoost()
		{
			if (Environment.MachineName == "ROMMELDIJK")
			{
				Database = "Joost";
			}

			string connection = $"Trusted_Connection=True;Data Source=(Local);" +
				$"Database={Database};MultipleActiveResultSets=true";
			int added = 0;
			int changed = 0;
			int skipped = 0;
			int excepts = 0;

			using (Db = new JournalDbContext(connection))
			{
				foreach (Journal record in VM.Contacts)
				{
					List<Journal> search = Db.Journals
						.Where(x => x.DTCreation == record.DTCreation &&
							x.Event == record.Event)
						.ToList();

					//Add a new record
					if (search.Count == 0)
					{
						Journal journal = new Journal()
						{
							DTStart = record.DTStart,
							DTCreation = record.DTCreation,
							Event = record.Event,
							Message = record.Message
						};
						Db.Journals.Add(journal);
						Db.SaveChanges();

						added++;
					}
					else if (search.Count == 1)
					{
						//Update an existing record
						if (search[0].Message != record.Message)
						{
							search[0].Message = record.Message;
							Db.SaveChanges();

							changed++;
						}
						//Skip update an existing record (message is already the same)
						else
						{
							skipped++;
						}
					}
					//More than 1 record wanted the same record to be updated
					else
					{
						excepts++;
					}
				}
			}//using

			if (excepts > 0)
			{
				MessageBox.Show($"{excepts} wanted the same record to be updated",
					$"Export to {Database}",
					MessageBoxButton.OK,
					MessageBoxImage.Exclamation);
				return;
			}

			MessageBox.Show($"{added} record(s) added\n" +
											$"{changed} record(s) change\n" +
											$"{skipped} record(s) skipped",
				$"Export to {Database}",
				MessageBoxButton.OK,
				MessageBoxImage.Information);

			if (Environment.MachineName == "ROMMELDIJK")
			{
				VM.ClearContacts();
			}
		}

	}
}
