using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantFrontend.Models;
using RestaurantFrontend.Models.DTOs;
using System.Diagnostics;

namespace RestaurantFrontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _client;
        private readonly string _baseUri = "https://localhost:7071/";


        public HomeController(ILogger<HomeController> logger,  HttpClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            List<MenuDTO> popularDishes = new List<MenuDTO>();
            try
            {
                var response = await _client.GetAsync($"{_baseUri}api/Menu");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var allDishes = JsonConvert.DeserializeObject<List<MenuDTO>>(json);

                    
                    popularDishes = allDishes.Where(dish => dish.MenuId >= 19 && dish.MenuId <= 22).ToList();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to load popular dishes.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading popular dishes: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Error loading popular dishes.");
            }

            return View(popularDishes); 
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    
}
