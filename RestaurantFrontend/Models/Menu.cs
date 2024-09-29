using System.ComponentModel.DataAnnotations;

namespace RestaurantFrontend.Models
{
    public class Menu
    {
        
        public int Id { get; set; }

        public string DishName { get; set; }
       
        public decimal Price { get; set; }

        public string Description {  get; set; }

        public bool IsAvailable { get; set; }
    }
}
