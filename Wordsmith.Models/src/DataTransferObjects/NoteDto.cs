using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.DataTransferObjects;

public class NoteDto
{
    [SwaggerSchema("ID of the note")]
    public int Id { get; set; }
    
    [SwaggerSchema("The exact section of the EPUB that the note refers to")]
    public string Cfi { get; set; }
    
    [SwaggerSchema("Content of the note")]
    public string Content { get; set; }
    
    [SwaggerSchema("Date when the note was made")]
    public DateTime DateAdded { get; set; }
    
    [SwaggerSchema("ID of the ebook that the note refers to")]
    public int EBookId { get; set; }
    
    [SwaggerSchema("ID of the user the note belongs to")]
    public int UserId { get; set; }
}