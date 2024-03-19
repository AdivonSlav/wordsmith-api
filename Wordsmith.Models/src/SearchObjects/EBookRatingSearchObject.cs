using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.SearchObjects;

public class EBookRatingSearchObject : SearchObject
{
    [SwaggerParameter("The rating value")]
    [Range(minimum: 1, maximum: 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int? Rating { get; set; }
    
    [SwaggerParameter("ID of the user that made the rating")]
    public int? UserId { get; set; }
    
    [SwaggerParameter("ID of the ebook that is rated")]
    public int? EBookId { get; set; }
}