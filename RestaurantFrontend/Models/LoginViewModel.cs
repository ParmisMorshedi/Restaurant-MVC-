using System.ComponentModel.DataAnnotations;

namespace RestaurantFrontend.Models
{
    public class LoginViewModel
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is mandatory.")]
        public string Password { get; set; }

        //public bool RememberMe { get; set; }
    }
}
