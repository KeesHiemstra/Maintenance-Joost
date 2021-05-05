using System;
using System.ComponentModel;

namespace Joost
{
	public interface IJournal : INotifyPropertyChanged
	{
		int LogID { get; set; }
		DateTime? DTStart { get; set; }
		string Message { get; set; }
		string Event { get; set; }
		DateTime? DTCreation { get; set; }
		byte[] RowVersion { get; set; }
	}
}
