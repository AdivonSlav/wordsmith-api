using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Wordsmith.DataAccess.Db.ValueConverters;

public class UtcValueConverter : ValueConverter<DateTime, DateTime>
{
    public UtcValueConverter()
        : base(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
    {
        
    }
}