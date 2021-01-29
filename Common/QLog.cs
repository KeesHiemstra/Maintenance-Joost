using CsvHelper.Configuration.Attributes;

using System;

namespace QuickLog
{
	public class QLog
	{
		[Name("Time")]
		public DateTime Time { get; set; }

		[Name("Event")]
		[Optional]
		public string Event { get; set; }

		[Name("Message")]
		public string Message { get; set; }
	}
}
