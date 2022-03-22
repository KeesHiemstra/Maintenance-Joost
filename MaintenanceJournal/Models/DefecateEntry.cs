using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceJournal.Models
{
	internal class DefecateEntry
	{
		public DateTime Time { get; set; }
		public byte NumberSheets { get; set; }
		public string Info { get; set; }
	}
}
