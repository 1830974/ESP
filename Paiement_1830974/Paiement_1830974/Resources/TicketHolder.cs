using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paiement_1830974.Models;

namespace Paiement_1830974.Resources
{
    public static class TicketHolder
    {
        public static Ticket CurrentTicket { get; set; }
        public static DateTime StayTime { get; set; }
    }
}
