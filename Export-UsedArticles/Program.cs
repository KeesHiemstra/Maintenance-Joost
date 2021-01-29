using Joost;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Export_UsedArticles
{
	class Program
	{
		private static string Database = "Joost_Dev";
		private static string OutputPath = "\\\\Rommeldijk\\Data\\Articles.json";

		public static JournalDbContext Db { get; private set; }

		static void Main(string[] args)
		{
			Console.WriteLine("Export-UsedArticles\n");
			if (Environment.MachineName == "ROMMELDIJK")
			{
				Database = "Joost";
			}

			ExportUserArticles();

			Console.Write("\nPress any key...");
			Console.ReadKey();
		}

		private static void ExportUserArticles()
		{
			string connection = $"Trusted_Connection=True;Data Source=(Local);" +
				$"Database={Database};MultipleActiveResultSets=true";

			List<Journal> journals = new List<Journal>();
			using (Db = new JournalDbContext(connection))
			{
				journals = (from j in Db.Journals
										where j.Event == "Aangebroken"
										orderby j.Message
										select j).ToList();
			}

			int count = 0;
			Dictionary<string, string> Translation = new Dictionary<string, string>();
			foreach (var item in journals.Select(x => x.Message).Distinct())
			{
				count++;
				Console.WriteLine($"{count}\t{item}");
				Translation.Add(item, item);
			}

			string json = JsonConvert.SerializeObject(Translation, Formatting.Indented);
			using (StreamWriter stream = new StreamWriter(OutputPath))
			{
				stream.Write(json);
			}
		}


	}
}
