using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.SearchObjects;

public class UserSearchObject : SearchObject
{
    [SwaggerParameter("Username of the user")]
    public string? Username { get; set; }
}