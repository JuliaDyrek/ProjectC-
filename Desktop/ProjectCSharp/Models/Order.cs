using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektRestauracja.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int ReservationId { get; set; }
        public int DishId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public Reservation? Reservation { get; set; }
        public Dish? Dish { get; set; }
    }

}


