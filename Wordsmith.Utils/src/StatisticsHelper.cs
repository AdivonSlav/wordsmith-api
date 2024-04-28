namespace Wordsmith.Utils;

public static class StatisticsHelper
{
    private const int StatisticsMaxDaysForHourlyGranularity = 5;
    
    public static bool IsHourlyGranularity(DateTime start, DateTime end)
    {
        return (end.Date - start.Date).TotalDays <= StatisticsMaxDaysForHourlyGranularity;
    }
    
    public static DateTime AdjustDateForGranularity(DateTime date, bool isHourlyGranularity)
    {
        return isHourlyGranularity
            ? new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0)
            : date.Date;
    }
    
    public static TimeSpan GetIncrementForGranularity(bool isHourlyGranularity)
    {
        return isHourlyGranularity ? TimeSpan.FromHours(1) : TimeSpan.FromDays(1);
    }
}