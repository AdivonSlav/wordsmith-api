using System.ComponentModel.DataAnnotations;
using Wordsmith.Models.DataTransferObjects;

namespace Wordsmith.Models.RequestObjects;

public class EBookInsertRequest
{
    [Required]
    public EBookParseDto ParsedInfo { get; set; }
    
    public decimal? Price { get; set; }
    
    [Required]
    public int AuthorId { get; set; }
    
    [Required]
    public List<int> GenreIds { get; set; }
    
    [Required]
    public int MaturityRatingId { get; set; }
    
    [Required]
    public string SavedBookName { get; set; }
}