using CsvHelper;
using CsvHelper.Configuration;

using Joost;

using QuickLog;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Import_QuickLog
{
	class Program
	{
		private static string Database = "Joost_Dev";
		private static string InputPath = "\\\\Rommeldijk\\Data\\QuickLog.csv";
		private static readonly CsvConfiguration Config =
			new CsvConfiguration(CultureInfo.CurrentUICulture)
			{
				Delimiter = "\t",
			};

		public static List<QLog> Logs { get; set; } = new List<QLog>();
		public static JournalDbContext Db { get; private set; }

		static void Main(string[] args)
		{
			Console.WriteLine("Import-QuickLog\n");
			if (Environment.MachineName == "ROMMELDIJK")
			{
				Database = "Joost";
			}

			ImportQuickLog();

			Console.Write("\nPress any key...");
			Console.ReadKey();
		}

		private static void ImportQuickLog()
		{
			if (!File.Exists(InputPath))
			{
				Console.WriteLine($"The file '{InputPath}' doesn't exist");
				return;
			}

			using (var reader = new StreamReader(InputPath))
			using (var cvs = new CsvReader(reader, Config))
			{
				var logs = cvs.GetRecords<QLog>();
				Logs = new List<QLog>(logs);
			}

			Console.WriteLine($"Read {Logs.Count} records");
			if (Logs.Count == 0) { return; }

			ProcessCsv();
		}

		private static void ProcessCsv()
		{
			string connection = $"Trusted_Connection=True;Data Source=(Local);" +
				$"Database={Database};MultipleActiveResultSets=true";

			int added = 0;
			int changed = 0;
			int skipped = 0;
			int excepts = 0;
			using (Db = new JournalDbContext(connection))
			{
				foreach (QLog record in Logs)
				{
					List<Journal> search = Db.Journals
						.Where(x => x.DTStart == record.Time &&
									 x.Event == record.Event)
						.ToList();

					if (search.Count == 0)
					{
						Journal journal = new Journal()
						{
							DTStart = record.Time,
							DTCreation = record.Time,
							Event = record.Event,
							Message = record.Message
						};
						Db.Journals.Add(journal);
						Db.SaveChanges();

						added++;
					}
					else
					{
						if (search.Count == 1)
						{
							if (search[0].Message != record.Message)
							{
								search[0].Message = record.Message;
								Db.SaveChanges();

								changed++;
							}
							else
							{
								skipped++;
							}
						}
						else
						{
							excepts++;
						}
					}
				}
			}

			Console.WriteLine($"Added {added} records");
			Console.WriteLine($"Changed {changed} records");
			Console.WriteLine($"Skipped {skipped} records");
			Console.WriteLine($"{excepts} lines caused to excepts");
		}

	}
}
