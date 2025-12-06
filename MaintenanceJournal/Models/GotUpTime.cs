using System;

namespace MaintenanceJournal.Models
{
	public class GotUpTime
	{
		public string Period { get; set; }
		public TimeSpan Avg { get; set; }
		public TimeSpan Min { get; set; }
		public TimeSpan Max { get; set; }
	}
}
