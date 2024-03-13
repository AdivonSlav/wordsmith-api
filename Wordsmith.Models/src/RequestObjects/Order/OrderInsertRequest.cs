namespace Wordsmith.Models.RequestObjects.Order;

public class OrderInsertRequest
{
    public int UserId { get; set; }
    
    public int EbookId { get; set; }
}