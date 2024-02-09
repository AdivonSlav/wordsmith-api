using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.SearchObjects;

public class UserLibraryCategorySearchObject : SearchObject
{
    [SwaggerParameter("ID of the user that the category belongs to")]
    public int? UserId { get; set; }
}