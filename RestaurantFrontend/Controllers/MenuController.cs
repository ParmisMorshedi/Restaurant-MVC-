using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantFrontend.Models;
using System.Text;

namespace RestaurantFrontend.Controllers
{
    public class MenuController : Controller
    {
        //Dependency Injection
        private readonly HttpClient _client;
        private string baseUri = "https://localhost:7071/";
        public MenuController(HttpClient client) 
        {
            _client = client;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Available Menu";

            var response = await _client.GetAsync($"{baseUri}api/Menu");

            if (!response.IsSuccessStatusCode)
            {
             
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                return View(new List<Menu>());
            }
            var json = await response.Content.ReadAsStringAsync();

            var menuList = JsonConvert.DeserializeObject<List<Menu>>(json);

            return View(menuList);
        }
    }
}
