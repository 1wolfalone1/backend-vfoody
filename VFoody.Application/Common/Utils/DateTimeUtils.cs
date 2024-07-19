namespace VFoody.Application.Common.Utils;

public static class DateTimeUtils
{
    public static DateTime SetTimeToMidnight(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
    }
}