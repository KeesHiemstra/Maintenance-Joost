using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceJournal.Models
{
	/// <summary>
	/// Use for the report.
	/// </summary>
	public class CoffeeUsage
	{
		public DateTime? Opened { get; set; }
		public bool NewOpened { get; set; }
		public int Days { get; set; }
		public DateTime LastOpened { get; set; }
		public int Cups { get; set; }
		public int ActualDays { get; set; }
		public decimal CupsPerDay { get; set; }
		public int CupsMin { get; set; }
		public int CupsMax { get; set; }
	}
}
