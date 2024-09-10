using Microsoft.AspNetCore.Mvc;

namespace RestaurantFrontend.Controllers
{
    public class CustomerController : Controller
    {
        private readonly HttpClient _client;
        private string baseUri = "https://localhost:7071/";
        public CustomerController(HttpClient client)
        {
            _client = client;
        }

        public IActionResult Index()
        {
            
            return View();
        }


    }
}
