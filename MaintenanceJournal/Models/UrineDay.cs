using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceJournal.Models
{
	internal class UrineDay
	{
		public DateTime Date { get; set; }
		public byte Total { get; set; }
		public byte Night { get; set; }
		public string Info { get; set; }
	}
}
