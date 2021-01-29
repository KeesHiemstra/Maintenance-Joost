using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceJournal.Models
{
	[Table("Contact")]
	public class Contact
	{
		[Required]
		[Key]
		public int ContactID { get; set; }

		[Required]
		[StringLength(40)]
		public string Name { get; set; }

		[Required]
		public Boolean IsPerson { get; set; }

		[StringLength(30)]
		public string City { get; set; }

		[StringLength(50)]
		[EmailAddress]
		public string Mail { get; set; }

		[StringLength(30)]
		[Phone]
		public string Phone { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public Nullable<System.DateTime> BirthDate { get; set; }

		[Required]
		public Boolean IsBirthYear { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public Nullable<DateTime> DeathDate { get; set; }

		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }
	}
}