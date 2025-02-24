namespace ParkingApp_1830974.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime? PaymentTime { get; set; }
        public string LicensePlate { get; set; }
        public string State { get; set; }

        public Ticket() { }

        public Ticket(int id, DateTime arrivalTime, DateTime paymentTime, string licensePlate, string state)
        {
            Id = id;
            ArrivalTime = arrivalTime;
            PaymentTime = paymentTime;
            LicensePlate = licensePlate;
            State = state;
        }
    }
}
