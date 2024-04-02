namespace Wordsmith.Models.DataTransferObjects;

public class EBookRatingDto
{
    public int Id { get; set; }
    
    public int Rating { get; set; }
    
    public DateTime RatingDate { get; set; }
    
    public int EBookId { get; set; }
    
    public int UserId { get; set; }
}