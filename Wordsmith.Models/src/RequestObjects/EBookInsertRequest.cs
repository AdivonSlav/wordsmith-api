using System.ComponentModel.DataAnnotations;
using Wordsmith.Models.ValidationAttributes;

namespace Wordsmith.Models.RequestObjects;

public class EBookInsertRequest
{
    [Required]
    [StringLength(20)]
    public string Title { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Description { get; set; }
    
    public decimal? Price { get; set; }
    
    [Base64Image]
    public string? EncodedCoverArt { get; set; }
    
    [Required]
    public int GenreId { get; set; }
    
    [Required]
    public int MaturityRatingId { get; set; }
}