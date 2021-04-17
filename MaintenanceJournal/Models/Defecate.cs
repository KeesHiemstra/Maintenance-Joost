using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceJournal.Models
{
	public class Defecate
	{
		public int DayNo { get; private set; }
		public DateTime Day { get; set; }
		public int Count { get; set; }
		public decimal Avg { get; set; }
		public int Min { get; set; }
		public int Max { get; set; }

		public Defecate()
		{
			DayNo++;
		}
	}
}
