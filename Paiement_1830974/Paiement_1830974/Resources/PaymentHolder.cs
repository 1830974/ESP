using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paiement_1830974.Resources
{
    public static class PaymentHolder
    {
        public static double BaseAmount { get; set; }
        public static double TPS { get; set; }
        public static double TVQ { get; set; }
        public static double TotalAmount { get; set; }
    }
}
