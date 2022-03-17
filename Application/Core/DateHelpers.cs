namespace Application.Core
{
    public static class DateHelpers
    {
        public static DateTime SetDateTimeToCurrent(DateTime date)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, "Europe/Warsaw");;
        }
        public static DateTime SetDateOnlyDaysMonthYear(DateTime date)
        {
            var newDate=new DateTime(date.Year, date.Month, date.Day);
            return new DateTime(date.Year, date.Month, date.Day);
        }
    }
}