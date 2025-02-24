namespace API_CIUSSS_1830974.Models
{
    public class Parking
    {
        public int Id { get; set; }
        public int AllSpaces { get; set; }
        public int OccupiedSpaces { get; set; }

        public Parking() { }

        public Parking(int id, int allSpaces, int occupiedSpaces)
        {
            Id = id;
            AllSpaces = allSpaces;
            OccupiedSpaces = occupiedSpaces;
        }
    }
}
