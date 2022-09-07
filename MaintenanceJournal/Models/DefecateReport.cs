using System;
using System.Security.Permissions;

namespace MaintenanceJournal.Models
{
	internal class DefecateReport
	{
		public DateTime Date { get; set; }
		public string Period { get; set; }
		public int TotalGoings { get; set; }
		public byte MinGoings { get; set; }
		public double AvgGoings { get; set; }
		public byte MaxGoings { get; set; }
		public int TotalWips { get; set; }
		public byte MinWips { get; set; }
		public double AvgWips { get; set; }
		public byte MaxWips { get; set; }
	}
}
