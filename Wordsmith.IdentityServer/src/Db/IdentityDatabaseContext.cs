using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wordsmith.IdentityServer.Db.Entities;

namespace Wordsmith.IdentityServer.Db;

public class IdentityDatabaseContext : IdentityDbContext
{
    public IdentityDatabaseContext(DbContextOptions<IdentityDatabaseContext> options)
        : base(options) { }

    public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
}