using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppBach3.Outils
{
    static class Extension
    {

        public static DateTime PremierJourSemaine(this DateTime dt)
        {
            int i = dt.DayOfWeek - DayOfWeek.Monday;
            dt = dt.AddDays(-i);
            return dt;
        }
    }
}
