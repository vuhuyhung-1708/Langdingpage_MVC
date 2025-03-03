using System.Data.Entity;

namespace website_test.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("SQLiteConnection")
        {
        }

        public DbSet<UserMessage> UserMessages { get; set; }
    }
}
