using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects.EBookRating;

public class EBookRatingInsertRequest
{
    [Required]
    [Range(minimum: 1, maximum: 5, ErrorMessage = "Ratings must be from 1 to 5")]
    public int Rating { get; set; }
    
    [Required]
    public int EBookId { get; set; }
    
    public int UserId { get; set; }
}