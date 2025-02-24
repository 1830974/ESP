namespace API_CIUSSS_1830974.Models.DTO
{
    public class TicketDTO
    {
        public string LicensePlate { get; set; }

        public TicketDTO() { }

        public TicketDTO(string licensePlate)
        {
            LicensePlate = licensePlate;
        }
    }
}
