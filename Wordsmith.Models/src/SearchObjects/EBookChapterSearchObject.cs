using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.SearchObjects;

public class EBookChapterSearchObject : SearchObject
{
    [SwaggerParameter("Name of the chapter")]
    public string? ChapterName { get; set; }
    
    [SwaggerParameter("ID of the ebook that the chapter belongs to")]
    public int? EBookId { get; set; }
}