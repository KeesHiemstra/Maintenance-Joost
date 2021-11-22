using Joost;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Export_Huishouden
{
	class Program
	{
		private static string Database = "Joost_Dev";
		private static readonly string OutputPath = "\\\\Rommeldijk\\Data\\Afval.json";

		public static JournalDbContext Db { get; private set; }

		static void Main(string[] args)
		{
			Console.WriteLine("Export-Huishouden\n");
			if (Environment.MachineName == "ROMMELDIJK")
			{
				Database = "Joost";
			}

			ExportHuishouden();

			Console.Write("\nPress any key...");
			Console.ReadKey();
		}

		private static void ExportHuishouden()
		{
			string connection = $"Trusted_Connection=True;Data Source=(Local);" +
				$"Database={Database};MultipleActiveResultSets=true";

			List<Journal> journals = new List<Journal>();
			using (Db = new JournalDbContext(connection))
			{
				journals = (from j in Db.Journals
										where (j.Event == "Huishouding" || j.Event == "Afval") &&
											(j.Message.Contains("Gft") || 
											j.Message.Contains("Glas") ||
											j.Message.Contains("Plastic") ||
											j.Message.Contains("Rest"))
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
			using StreamWriter stream = new StreamWriter(OutputPath);
			stream.Write(json);
		}

	}
}
