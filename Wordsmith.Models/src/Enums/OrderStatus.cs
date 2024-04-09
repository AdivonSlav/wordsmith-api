using System.ComponentModel;

namespace Wordsmith.Models.Enums;

public enum OrderStatus
{
    [Description("Order is pending capture")]
    Pending,
    
    [Description("Order has been completed")]
    Completed,
    
    [Description("Order has been refunded")]
    Refunded
}