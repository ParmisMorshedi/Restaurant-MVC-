using System.ComponentModel.DataAnnotations;

namespace RestaurantFrontend.Models.DTOs
{
    public class ReservationDTO
    {
        public int ReservationId { get; set; }

        public int TableId { get; set; }

        public int CustomerId { get; set; }

        public TimeSpan Time { get; set; }
        public DateTime Date { get; set; }


        public int NumberOfGuests { get; set; }
    }
}
