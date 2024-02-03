using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.SearchObjects;

public class UserLibrarySearchObject : SearchObject
{
    [SwaggerParameter("The ID of the user that owns the library")]
    public int? UserId { get; set; }
}