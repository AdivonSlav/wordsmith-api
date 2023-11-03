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
    public int GenreId { get; set; }
    
    [Required]
    public int MaturityRatingId { get; set; }
    
    [Required]
    public string SavedBookName { get; set; }
}