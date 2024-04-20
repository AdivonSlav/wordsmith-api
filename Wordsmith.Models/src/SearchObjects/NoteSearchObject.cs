using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.SearchObjects;

public class NoteSearchObject : SearchObject
{
    [SwaggerParameter("ID of the user that made the note")]
    public int? UserId { get; set; }
    
    [SwaggerParameter("ID of the ebook where the note was made")]
    public int? EBookId { get; set; }
}