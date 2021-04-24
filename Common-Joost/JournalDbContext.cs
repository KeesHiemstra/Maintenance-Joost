using System.Data.Entity;

namespace Joost
{
	public class JournalDbContext : DbContext
	{
		public JournalDbContext(string dbConnection) : base(dbConnection) { }

		public DbSet<Journal> Journals { get; set; }
	}
}
