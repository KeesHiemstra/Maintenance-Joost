using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceJournal.Models
{
	public class Calendar
	{
		public int Week { get; set; }
		public DateTime Date { get; set; }
		public string Mo { get; set; }
		public string Tu { get; set; }
		public string We { get; set; }
		public string Th { get; set; }
		public string Fr { get; set; }
		public string Sa { get; set; }
		public string Su { get; set; }

		public Calendar(DateTime date, int week)
		{
			Date = date;
			Week = week;
		}
	}
}
