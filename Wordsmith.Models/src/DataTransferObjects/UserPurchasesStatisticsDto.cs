using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.DataTransferObjects;

public class UserPurchasesStatisticsDto
{
    [SwaggerSchema("Username of the user")]
    public string Username { get; set; }
    
    [SwaggerSchema("Number of ebooks the user purchased")]
    public long PurchaseCount { get; set; }
    
    [SwaggerSchema("Total amount of money the user spent on purchases")]
    public decimal TotalSpent { get; set; }
}