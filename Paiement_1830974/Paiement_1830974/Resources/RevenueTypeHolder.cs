using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paiement_1830974.Resources
{
    public static class RevenueTypeHolder
    {
        public static DateTime Monday { get; set; }
        public static double Amount { get; set; }
        public static string RevenueType { get; set; } = string.Empty;

        public static DateTime getLastMonday()
            => DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
    }
}
