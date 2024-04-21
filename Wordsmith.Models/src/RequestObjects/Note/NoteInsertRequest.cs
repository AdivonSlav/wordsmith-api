using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.RequestObjects.Note;

public class NoteInsertRequest
{
    [SwaggerSchema("Content of the note")]
    [Required]
    [StringLength(maximumLength: 400, ErrorMessage = "The content of the note must not exceed 400 characters")]
    public string Content { get; set; }
    
    [SwaggerSchema("ID of the ebook where the note is being made")]
    [Required]
    public int EBookId { get; set; }
    
    [SwaggerSchema("The exact section of the EPUB that the note refers to")]
    [Required]
    public string Cfi { get; set; }
    
    [SwaggerSchema("The text of the ebook that the note refers to")]
    [Required]
    [StringLength(maximumLength: 1000, ErrorMessage = "The referenced text of the note must not exceed 1000 characters")]
    public string ReferencedText { get; set; }
    
    public int UserId { get; set; }
}