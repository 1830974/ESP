namespace API_CIUSSS_1830974.Models
{
    public class Reciept
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public DateTime StayTime { get; set; }
        public double Total { get; set; }
        public double TPS { get; set; }
        public double TVQ { get; set; }

        public Reciept() { }

        public Reciept(int id, int ticketId, Ticket ticket, DateTime stayTime, double total, double tPS, double tVQ)
        {
            Id = id;
            TicketId = ticketId;
            Ticket = ticket;
            StayTime = stayTime;
            Total = total;
            TPS = tPS;
            TVQ = tVQ;
        }
    }
}
