using System;

namespace MaintenanceJournal.Models
{
	/// <summary>
	/// Use for the report.
	/// </summary>
	public class OpenedArticles
	{
		public string Article { get; set; }
		public DateTime? Opened { get; set; }
		public DateTime? Closed { get; set; } //Added v2.1.0.29
		public string Number { get; set; }
		public int Days { get; set; }
		public int Count { get; set; }
		public double Avg { get; set; } //Average number days
		public int Min { get; set; } 
		public int Max { get; set; }
	}
}
