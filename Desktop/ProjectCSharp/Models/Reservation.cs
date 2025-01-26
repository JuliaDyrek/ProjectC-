namespace ProjektRestauracja.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime ReservationDate { get; set; }
        public int NumberOfPeople { get; set; }
        public TimeSpan ReservationTime { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }

}
