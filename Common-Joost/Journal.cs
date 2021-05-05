using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Joost
{
	[Table("Journal")]
	public class Journal : IJournal
	{
		#region [ Fields ]

		private int _LogId;
		private DateTime? _DTStart;
		private string _Message;
		private string _Event;
		private DateTime? _DTCreation;
		private byte[] _RowVersion;

		#endregion

		#region [ Properties ]

		[Key]
		[Required]
		[Display(Name = "ID")]
		/// <summary>
		/// Unique log identification.
		/// </summary>
		public int LogID
		{
			get => _LogId;
			set
			{
				if (_LogId != value)
				{
					_LogId = value;
					NotifyPropertyChanged("LogId");
				}
			}
		}
		[Display(Name = "Date")]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}", ApplyFormatInEditMode = true)]
		/// <summary>
		/// <see cref="Nullable"/> manual start date/time.
		/// </summary>
		public DateTime? DTStart
		{
			get => _DTStart;
			set
			{
				if (_DTStart != value)
				{
					_DTStart = value;
					NotifyPropertyChanged("DTStart");
				}
			}
		}
		[Required]
		[StringLength(512)]
		/// <summary>
		/// Log message, can't be empty.
		/// </summary>
		public string Message
		{
			get => _Message;
			set
			{
				if (_Message != value)
				{
					_Message = value;
					NotifyPropertyChanged("Message");
				}
			}
		}
		[StringLength(40)]
		[DisplayFormat(NullDisplayText = "<No event>")]
		/// <summary>
		/// Event name, can be empty.
		/// </summary>
		public string Event
		{
			get => _Event;
			set
			{
				if (_Event != value)
				{
					_Event = value;
					NotifyPropertyChanged("Event");
				}
			}
		}
		[Display(Name = "Create date")]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = false)]
		/// <summary>
		/// Automatic creation date/time.
		/// </summary>
		public DateTime? DTCreation
		{
			get => _DTCreation;
			set
			{
				if (_DTCreation != value)
				{
					_DTCreation = value;
					NotifyPropertyChanged("DTCreation");
				}
			}
		}
		[ConcurrencyCheck]
		[Timestamp]
		/// <summary>
		/// Automatic row version.
		/// </summary>
		public byte[] RowVersion
		{
			get => _RowVersion;
			set
			{
				if (_RowVersion != value)
				{
					_RowVersion = value;
					NotifyPropertyChanged("RowVersion");
				}
			}
		}

		#endregion

		#region [ Notification ]

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

	}
}
