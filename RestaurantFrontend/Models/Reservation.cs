using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestaurantFrontend.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public int CustomerId { get; set; }
        public TimeOnly Time { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfGuests { get; set; }
    }
}
