using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.DB.Entities;

namespace Wordsmith.DataAccess.DB
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ReportReason> ReportReasons { get; set; }
    }
}