using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJBudgetWBackend.Commun
{
    public static class Tools
    {
        public static (DateTime, DateTime)? GetFirstAndLastDayMonthOfThisYear(byte month)
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
