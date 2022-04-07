using System;

namespace NJBudgetWBackend.Commun
{
    public static class Tools
    {
        public static (DateTime first, DateTime last)? GetFirstAndLastDayMonthOfThisYear(byte month)
        {
            if (month == 0 || month > 12)
            {
                return null;
            }
            //1-
            var firstMonthDay = new DateTime(DateTime.Now.Year, month, 1);
            var lastMonthDay = firstMonthDay.AddMonths(1).AddDays(-1);
            return (firstMonthDay, lastMonthDay);
        }
    }
}
