namespace Administration.Models
{
    public class Prices
    {
        public int Id { get; set; }
        public DateTime SetPriceDate { get; set; }
        public string PriceName { get; set; }
        public double Price { get; set; }

        public Prices() { }

        public Prices(int id, DateTime setPriceDate, string priceName, double price)
        {
            Id = id;
            SetPriceDate = setPriceDate;
            PriceName = priceName;
            Price = price;
        }
    }
}
