using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SND.Utils
{
    public static class DateHandler
    {
        public static DateTime ParseDate(string date)
        {
            if (date.Length == 19)
            {

                return DateTime.ParseExact(date,
                                    "yyyy-MM-dd hh:mm:ss",
                                    System.Globalization.CultureInfo.InvariantCulture);

            }
            else if (date.Length == 10)
            {
                return DateTime.ParseExact(date,
                                   "yyyy-MM-dd",
                                   System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (date.Length == 21)
            {
                return DateTime.ParseExact(date,
                                   "yyyy-MM-dd hh:mm:ss tt",
                                   System.Globalization.CultureInfo.InvariantCulture);
            }
            return DateTime.Now;
        }
    }
}