namespace Lab2.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime GetWeekStart(this DateTime date)
        {
            var daysFromMonday = ((int)date.DayOfWeek - 1 + 7) % 7;
            return date.Date.AddDays(-daysFromMonday);
        }

        public static DateTime GetWeekEnd(this DateTime date)
        {
            return date.GetWeekStart().AddDays(7);
        }

        public static DateTime GetMonthStart(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime GetMonthEnd(this DateTime date)
        {
            return date.GetMonthStart().AddMonths(1);
        }
    }
}