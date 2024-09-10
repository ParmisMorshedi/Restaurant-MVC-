using System.ComponentModel.DataAnnotations;

namespace RestaurantFrontend.Models
{
    public class Table
    {

        public int Id { get; set; }

        public int Number { get; set; }

        public int Seats { get; set; }
    }
}
