namespace Wordsmith.Utils.StatisticsHelper;

public static class StatisticsHelper
{
    public static IEnumerable<DateTime> GetAllMonthsInRange(StatisticsRequest request)
    {
        var startDate = new DateTime(request.StartDate.Year, request.StartDate.Month, 1);
        var endDate = new DateTime(request.EndDate.Year, request.EndDate.Month, 1).AddMonths(1).AddDays(-1); // Set end date to the last day of the month
        
        var allMonths = new List<DateTime>();
        for (var date = startDate; date <= endDate; date = date.AddMonths(1))
        {
            allMonths.Add(date);
        }
        
        return allMonths;
    }
}