using System.ComponentModel;

namespace Wordsmith.Models.Enums;

public enum ReportType
{
    [Description("Report is made towards a user")]
    User,
    
    [Description("Report is made towards an ebook")]
    Ebook,
    
    [Description("Report is made towards the application")]
    App
}