using Joost;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;

namespace Import_TranslatedUsedArticles
{
	class Program
	{
		private static string Database = "Joost_Dev";
		private static readonly string InputPath = "\\\\Rommeldijk\\Data\\Articles.json";

		public static JournalDbContext Db { get; private set; }
		public static Dictionary<string, string> Articles { get; set; }

		static void Main(string[] args)
		{
			Console.WriteLine("Import-TranslatedUsedArticles\n");
			if (Environment.MachineName == "ROMMELDIJK")
			{
				Database = "Joost";
			}

			ImportArticles();

			Console.Write("\nPress any key...");
			Console.ReadKey();
		}

		private static void ImportArticles()
		{
			using (StreamReader stream = File.OpenText(InputPath))
			{
				string json = stream.ReadToEnd();
				Articles = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
			}

			Translate();
		}

		private static void Translate()
		{
			string connection = $"Trusted_Connection=True;Data Source=(Local);" +
				$"Database={Database};MultipleActiveResultSets=true";

			string sql;
			int result = -1;
			int total = 0;
			using (Db = new JournalDbContext(connection))
			{
				foreach (var item in Articles)
				{
					sql = $"UPDATE Journal " +
						$"SET [Message] = '{item.Value}' " +
						$"WHERE [Event] = 'Aangebroken' " +
						$"AND [Message] = '{item.Key}';";
					try
					{
						result = Db.Database.ExecuteSqlCommand(sql);
						total += result;
					}
					catch (Exception ex)
					{
						Console.WriteLine($"SQL: {sql}\ncaused and error: {ex.Message}");
						return;
					}
				}
				Console.WriteLine($"\n{total} records are changed");
			}
		}

	}
}
