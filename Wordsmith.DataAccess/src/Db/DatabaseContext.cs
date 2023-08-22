using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db.Entities;

namespace Wordsmith.DataAccess.Db;

public class DatabaseContext : DbContext
{
    public DatabaseContext() { }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options) { }

    public virtual DbSet<ReportReason> ReportReasons { get; set; }
    public virtual DbSet<EBookReport> EBookReports { get; set; }
    public virtual DbSet<ReportDetails> ReportDetails { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserReport> UserReports { get; set; }
    public virtual DbSet<Image> Images { get; set; }
}