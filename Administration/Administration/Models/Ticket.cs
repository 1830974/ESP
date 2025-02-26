using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime? PaymentTime { get; set; }
        public string LicensePlate { get; set; }
        public string State { get; set; }

        public Ticket() 
        {
            Id = 0;
            ArrivalTime = DateTime.Now;
            PaymentTime = DateTime.Now;
            LicensePlate = string.Empty;
            State = string.Empty;
        }

        public Ticket(int id, DateTime arrivelTime, DateTime paymentTime, string licensePlate, string state)
        {
            Id = id;
            ArrivalTime = arrivelTime;
            PaymentTime = paymentTime;
            LicensePlate = licensePlate;
            State = state;
        }
    }
}
