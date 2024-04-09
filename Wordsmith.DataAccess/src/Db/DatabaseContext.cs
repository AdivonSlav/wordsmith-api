using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Db.ValueConverters;
using Wordsmith.Models.Enums;

namespace Wordsmith.DataAccess.Db;

public class DatabaseContext : DbContext
{
    public DatabaseContext() { }

    public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration configuration)
        : base(options)
    {
    }

    public virtual DbSet<AppReport> AppReports { get; set; }
    public virtual DbSet<AuthorFollow> AuthorFollows { get; set; }
    public virtual DbSet<Comment> Comments { get; set; }
    public virtual DbSet<EBook> EBooks { get; set; }
    public virtual DbSet<EBookChapter> EBookChapters { get; set; }
    public virtual DbSet<EBookPromotion> EBookPromotions { get; set; }
    public virtual DbSet<EBookRating> EBookRatings { get; set; }
    public virtual DbSet<EBookReport> EBookReports { get; set; }
    public virtual DbSet<EBookGenre> EBookGenres { get; set; }
    public virtual DbSet<FavoriteEBook> FavoriteEBooks { get; set; }
    public virtual DbSet<Genre> Genres { get; set; }
    public virtual DbSet<Image> Images { get; set; }
    public virtual DbSet<MaturityRating> MaturityRatings { get; set; }
    public virtual DbSet<Note> Notes { get; set; }
    public virtual DbSet<ReportDetails> ReportDetails { get; set; }
    public virtual DbSet<ReportReason> ReportReasons { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserBan> UserBans { get; set; }
    public virtual DbSet<UserLibrary> UserLibraries { get; set; }
    public virtual DbSet<UserLibraryCategory> UserLibraryCategories { get; set; }
    public virtual DbSet<UserReport> UserReports { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<CommentLike> CommentLikes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().Property(e => e.Status)
            .HasConversion(v => v.ToString(), v => Enum.Parse<OrderStatus>(v));
        modelBuilder.Entity<User>().Property(e => e.Status)
            .HasConversion(v => v.ToString(), v => Enum.Parse<UserStatus>(v));
        modelBuilder.Entity<ReportReason>().Property(e => e.Type)
            .HasConversion(v => v.ToString(), v => Enum.Parse<ReportType>(v));

        modelBuilder.Entity<Comment>().Property(e => e.LikeCount).HasDefaultValue(0);
        modelBuilder.Entity<User>().Property(e => e.Status).HasDefaultValue(UserStatus.Active);
        
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveConversion(typeof(UtcValueConverter));
    }
}

