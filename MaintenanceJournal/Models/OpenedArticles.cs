using System;

namespace MaintenanceJournal.Models
{
	/// <summary>
	/// Use for the report.
	/// </summary>
	public class OpenedArticles
	{
		public DateTime? Opened { get; set; }
		public int Days { get; set; }
		public string Number { get; set; }
	}
}
