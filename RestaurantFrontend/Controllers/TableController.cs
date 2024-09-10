using Microsoft.AspNetCore.Mvc;

namespace RestaurantFrontend.Controllers
{
    public class TableController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
