using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db.Entities;

namespace Wordsmith.DataAccess.Db;

public class DatabaseContext : DbContext
{
    public DatabaseContext() { }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options) { }

    public virtual DbSet<ReportReason> ReportReasons { get; set; }
}