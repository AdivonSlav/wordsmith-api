using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.DataTransferObjects;

public class EBookChapterDto
{
    [SwaggerSchema("ID of the ebook chapter")]
    public int Id { get; set; }
    
    [SwaggerSchema("Name of the ebook chapter")]
    public string ChapterName { get; set; }
    
    [SwaggerSchema("Number of the ebook chapter")]
    public int ChapterNumber { get; set; }
    
    [SwaggerSchema("ID of the ebook that the chapter belongs to")]
    public int EBookId { get; set; }
}