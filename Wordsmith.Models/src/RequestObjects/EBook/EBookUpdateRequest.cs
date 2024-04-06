using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.RequestObjects.EBook;

public class EBookUpdateRequest
{
    [SwaggerSchema("Title of the ebook")]
    [StringLength(maximumLength: 40)]
    public string Title { get; set; }
    
    [SwaggerSchema("Whether the ebook is hidden from viewing or not")]
    public bool IsHidden { get; set; }
}