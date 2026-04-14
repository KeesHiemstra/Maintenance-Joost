using System;

namespace MaintenanceJournal.Models
{
	public class Ejaculation 
	{
		public DateTime Date { get; set; }
		public int Total { get; set; }
	}

	public class EjaculationReport
	{
		public string Date { get; set; }
		public int Total { get; set; }
	}
}
