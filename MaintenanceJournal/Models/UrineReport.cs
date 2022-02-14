using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceJournal.Models
{
	internal class UrineReport
	{
		public string Period { get; set; }
		public byte TotalMin { get; set; }
		public double TotalAvg { get; set; }
		public byte TotalMax { get; set; }
		public byte DayMin { get; set; }
		public double DayAvg { get; set; }
		public byte DayMax { get; set; }
		public byte NightMin { get; set; }
		public double NightAvg { get; set; }
		public byte NightMax { get; set; }
	}
}
