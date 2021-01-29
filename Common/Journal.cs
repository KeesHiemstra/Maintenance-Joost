using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Joost
{
	[Table("Journal")]
	public class Journal
	{
		[Key]
		[Required]
		[Display(Name = "ID")]
		public int LogID { get; set; }

		[Display(Name = "Date")]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}", ApplyFormatInEditMode = true)]
		public DateTime? DTStart { get; set; }

		[Required]
		[StringLength(512)]
		public string Message { get; set; }

		[StringLength(40)]
		[DisplayFormat(NullDisplayText = "<No event>")]
		public string Event { get; set; }

		//[Required]
		//[Editable(false)]
		[Display(Name = "Create date")]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = false)]
		//[DefaultValue(DateTime, DateTime.Now.ToString())]
		public DateTime? DTCreation { get; set; }

		[ConcurrencyCheck]
		[Timestamp]
		public byte[] RowVersion { get; set; }
	}
}
