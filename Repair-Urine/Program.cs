using Joost;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair_Urine
{
	internal class Program
	{
		private static string Database = "Joost_Dev";
		public static JournalDbContext Db { get; private set; }

		public static int Missed;
		public static int Changed;

		static void Main(string[] args)
		{
			Console.WriteLine("Repair-Urine\n");
			if (Environment.MachineName == "ROMMELDIJK")
			{
				Database = "Joost";
			}

			RepairJoost();

			Console.Write("\nPress any key...");
			Console.ReadKey();
		}

		private static void RepairJoost()
		{
			string connection = $"Trusted_Connection=True;Data Source=(Local);" +
				$"Database={Database};MultipleActiveResultSets=true";

			using (Db = new JournalDbContext(connection))
			{
				List<Journal> toRepair = Db.Journals
					.Where(x => x.Event == "Kleine boodschap")
					.OrderByDescending(x => x.DTCreation)
					.ToList();

				foreach (Journal item in toRepair)
				{
					RepairRecord(item);
				}
			} //using

			Console.WriteLine($"Missed {Missed} records");
			Console.WriteLine($"Changed {Changed} records");
		}

		private static void RepairRecord(Journal item)
		{
			DateTime peeTime;
			DateTime date = item.DTCreation.Value.Date.AddDays(1);

			DateTime nextDay = date.AddDays(1);
			List<Journal> exist = Db.Journals
				.Where(x => x.DTStart.Value >= date &&
							 x.DTStart.Value < nextDay &&
							 x.Event == "Opgestaan")
				.ToList();
			if (exist.Count == 0)
			{
				//No record found
				peeTime = date.AddMinutes(500);
				Missed++;
			}
			else
			{
				peeTime = exist[0].DTStart.Value.AddMinutes(-1);
			};

			Console.WriteLine($"{item.DTCreation} => {peeTime}");

			item.DTStart = peeTime;
			item.DTCreation = peeTime;
			Db.SaveChanges();
			Changed++;
		}
	}
}
