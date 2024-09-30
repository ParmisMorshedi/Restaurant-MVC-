using System.ComponentModel.DataAnnotations;

namespace RestaurantFrontend.Models.DTOs
{
    public class MenuDTO
    {
        public int MenuId { get; set; }      
        public string DishName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
      
    }
}
