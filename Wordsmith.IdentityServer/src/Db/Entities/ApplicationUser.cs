using Microsoft.AspNetCore.Identity;

namespace Wordsmith.IdentityServer.Db.Entities;

public class ApplicationUser : IdentityUser
{
    public int? UserRefId { get; set; }
}