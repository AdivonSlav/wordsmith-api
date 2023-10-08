using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Wordsmith.Models.RequestObjects;

public class EBookInsertRequest
{
    [StringLength(20)]
    public string Title { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Description { get; set; }
    
    public decimal? Price { get; set; }
    
    [Required]
    public ImageInsertRequest CoverArt { get; set; }
    
    [Required]
    public int AuthorId { get; set; }
    
    [Required]
    public int GenreId { get; set; }
    
    [Required]
    public int MaturityRatingId { get; set; }
    
    public IFormFile File { get; set; }
}