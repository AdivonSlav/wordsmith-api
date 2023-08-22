using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects;

public class ImageInsertRequest
{
    [Required]
    public string EncodedImage { get; set; }
    
    [Required]
    public string Format { get; set; }
    
    public int? Size { get; set; }
}