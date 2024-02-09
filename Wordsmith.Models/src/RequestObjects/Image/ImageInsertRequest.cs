using System.ComponentModel.DataAnnotations;
using Wordsmith.Models.ValidationAttributes;

namespace Wordsmith.Models.RequestObjects.Image;

public class ImageInsertRequest
{
    [Required]
    [Base64Image]
    public string EncodedImage { get; set; }
    
    [Required]
    public string Format { get; set; }
    
    public int? Size { get; set; }
}