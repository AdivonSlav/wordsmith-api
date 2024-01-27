using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Wordsmith.Models.ValidationAttributes;

namespace Wordsmith.Models.RequestObjects;

public class EBookInsertRequest
{
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required]
    public string EncodedCoverArt { get; set; }
    
    [Required]
    public List<string> Chapters { get; set; }
    
    public decimal? Price { get; set; }
    
    [Required]
    public int AuthorId { get; set; }
    
    [Required]
    public List<int> GenreIds { get; set; }
    
    [Required]
    public int MaturityRatingId { get; set; }
    
    [Required]
    [EpubFile]
    public IFormFile file { get; set; }
}