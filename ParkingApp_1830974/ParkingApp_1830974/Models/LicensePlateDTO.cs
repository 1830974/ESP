namespace ParkingApp_1830974.Models
{
    public class LicensePlateDTO
    {
        public string LicensePlate { get; set; }
        public DateTime ArrivalTime { get; set; }

        public LicensePlateDTO() { }

        public LicensePlateDTO(string licensePlate)
        {
            LicensePlate = licensePlate;
        }

        public LicensePlateDTO (Ticket ticket)
        {
            LicensePlate = ticket.LicensePlate;
        }
    }
}
