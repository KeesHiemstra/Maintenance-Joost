using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceJournal.Models
{
	internal class DefecateDay
	{
		public DateTime Date { get; set; }
		public byte NumberEntries { get; set; }
		public byte TotalSheets { get; set; }
		public byte MinSheets { get; set; }
		public double AvgSheets { get; set; }
		public byte MaxSheets { get; set; }
	}
}
