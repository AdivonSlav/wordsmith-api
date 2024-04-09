using System.ComponentModel;

namespace Wordsmith.Models.Enums;

public enum UserStatus
{
    [Description("User is active")]
    Active,
    
    [Description("User has been temporarily banned")]
    TemporarilyBanned,
    
    [Description("User is permanently banned")]
    Banned,
}