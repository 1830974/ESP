namespace API_CIUSSS_1830974.Models
{
    public class RevenueByType
    {
        public int Id { get; set; }
        public DateTime Week { get; set; }
        public double Hourly { get; set; }
        public double HalfDay {get; set;}
        public double FullDay { get; set;}

        public RevenueByType()
        {

        }

        public RevenueByType(int id, DateTime week, double hourly, double halfDay, double fullDay)
        {
            Id = id;
            Week = week;
            Hourly = hourly;
            HalfDay = halfDay;
            FullDay = fullDay;
        }
    }
}
