using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db.Entities;

namespace Wordsmith.DataAccess.Db;

public class DatabaseContext : DbContext
{
    public DatabaseContext() { }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options) { }

    public virtual DbSet<AppReport> AppReports { get; set; }
    public virtual DbSet<AuthorFollow> AuthorFollows { get; set; }
    public virtual DbSet<Comment> Comments { get; set; }
    public virtual DbSet<EBook> EBooks { get; set; }
    public virtual DbSet<EBookChapter> EBookChapters { get; set; }
    public virtual DbSet<EBookPromotion> EBookPromotions { get; set; }
    public virtual DbSet<EBookRating> EBookRatings { get; set; }
    public virtual DbSet<EBookReport> EBookReports { get; set; }
    public virtual DbSet<EBookSale> EBookSales { get; set; }
    public virtual DbSet<FavoriteEBook> FavoriteEBooks { get; set; }
    public virtual DbSet<Genre> Genres { get; set; }
    public virtual DbSet<Image> Images { get; set; }
    public virtual DbSet<MaturityRating> MaturityRatings { get; set; }
    public virtual DbSet<Note> Notes { get; set; }
    public virtual DbSet<ReportDetails> ReportDetails { get; set; }
    public virtual DbSet<ReportReason> ReportReasons { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserBan> UserBans { get; set; }
    public virtual DbSet<UserLibrary> UserLibraries { get; set; }
    public virtual DbSet<UserReport> UserReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthorFollow>().HasKey(e => new { e.AuthorUserId, e.UserId });
        modelBuilder.Entity<FavoriteEBook>().HasKey(e => new { e.EBookId, e.UserId });
        modelBuilder.Entity<UserLibrary>().HasKey(e => new { e.EBookId, e.UserId });
        
        base.OnModelCreating(modelBuilder);
    }
}