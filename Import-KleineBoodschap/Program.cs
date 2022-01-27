using CHi.Extensions;

using CsvHelper;
using CsvHelper.Configuration;

using Joost;

using QuickLog;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Import_KleineBoodschap
{
	internal class Program
	{
		private static string Database = "Joost_Dev";
		private static string InputPath = "%OneDrive%\\Data\\KleineBoodschap.csv";
		private static readonly CsvConfiguration Config =
			new CsvConfiguration(CultureInfo.CurrentUICulture)
			{
				Delimiter = "\t",
			};

		public static List<QLog> Logs { get; set; } = new List<QLog>();
		public static JournalDbContext Db { get; private set; }

		static void Main(string[] args)
		{
			Console.WriteLine("Import-KleineBoodschap\n");
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
			if (!File.Exists(InputPath.TranslatePath()))
			{
				Console.WriteLine($"The file '{InputPath}' doesn't exist");
				return;
			}

			using (var reader = new StreamReader(InputPath.TranslatePath()))
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
			int missed = 0;
			using (Db = new JournalDbContext(connection))
			{
				foreach (QLog record in Logs)
				{
					DateTime peeTime;
					//A record is a date without time, search for a date of 'Opgestaan', substrack a minute
					DateTime nextDay = record.Time.AddDays(1);
					List<Journal> exist = Db.Journals
						.Where(x => x.DTStart.Value >= record.Time &&
									 x.DTStart.Value < nextDay &&
									 x.Event == "Opgestaan")
						.ToList();
					if (exist.Count == 0)
					{
						//No record found
						peeTime = record.Time.AddMinutes(500);
						missed++;
					}
					else
					{
						peeTime = exist[0].DTStart.Value.AddMinutes(-1);
					};

					Journal piddle = new Journal()
					{
						DTStart = peeTime,
						DTCreation = peeTime,
						Event = record.Event,
						Message = record.Message,
					};

					List<Journal> search = Db.Journals
						.Where(x => x.DTStart == piddle.DTStart &&
									 x.Event == piddle.Event)
						.ToList();

					if (search.Count == 0)
					{
						Journal journal = new Journal()
						{
							DTStart = piddle.DTStart,
							DTCreation = piddle.DTCreation,
							Event = piddle.Event,
							Message = piddle.Message
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

			Console.WriteLine($"Missed {missed} records");
			Console.WriteLine($"Added {added} records");
			Console.WriteLine($"Changed {changed} records");
			Console.WriteLine($"Skipped {skipped} records");
			Console.WriteLine($"{excepts} lines caused to excepts");
		}

	}
}
