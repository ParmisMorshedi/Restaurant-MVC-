using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantFrontend.Models;
using RestaurantFrontend.Models.DTOs;
using System.Net.Http.Headers;
using System.Text;


namespace RestaurantFrontend.Controllers
{
    public class MenuController : Controller
    {
        //Dependency Injection
        private readonly HttpClient _client;
        private string _baseUri = "https://localhost:7071/";
        public MenuController(HttpClient client)
        {
            _client = client;
        }
        [Authorize]
        public async Task<IActionResult> Index(string searchTitle, int? price)
        {
            ViewData["Title"] = "Available Menu";

            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"{_baseUri}api/Menu");

            if (!response.IsSuccessStatusCode)
            {

                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                return View(new List<MenuDTO>());
            }
            var json = await response.Content.ReadAsStringAsync();

            var menuList = JsonConvert.DeserializeObject<List<MenuDTO>>(json);
            if (!string.IsNullOrEmpty(searchTitle))
            {
                menuList = menuList.Where(m =>m.DishName.Contains(searchTitle)).ToList();
            }
            // Filter by price
            if (price.HasValue)
            {
                menuList = menuList.Where(m => m.Price <= price.Value).ToList();
            }

       


            return View(menuList);
        }
       
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMenu(MenuDTO menuDTO)
        {
            var token = HttpContext.Session.GetString("JwtToken");

            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonContent = JsonConvert.SerializeObject(menuDTO);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"{_baseUri}api/Menu", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to create menu.");
            return View(new List<MenuDTO>());
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
           
            var response = await _client.GetAsync($"{_baseUri}api/Menu/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var menu = JsonConvert.DeserializeObject<MenuDTO>(json);
                return View(menu);
            }

            ModelState.AddModelError(string.Empty, "Failed to load menu for updating.");
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> UpdateMenu(MenuDTO menuDTO)
        {
           
            var token = HttpContext.Session.GetString("JwtToken");

            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonContent = JsonConvert.SerializeObject(menuDTO);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"{_baseUri}api/Menu/{menuDTO.MenuId}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to update menu.");
            return View(new List<MenuDTO>());
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var token = HttpContext.Session.GetString("JwtToken");

            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _client.DeleteAsync($"{_baseUri}api/Menu/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to delete menu.");
            return RedirectToAction("Index");
        }
    }
}

