namespace API_CIUSSS_1830974.Models.DTO
{
    public class LicensePlateDTO
    {
        public string LicensePlate { get; set; }

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
