using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantFrontend.Models;
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
        public async Task<IActionResult> Index(string searchTitle, int? searchYear)
        {
            ViewData["Title"] = "Available Menu";

            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"{_baseUri}api/Menu");

            if (!response.IsSuccessStatusCode)
            {

                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                return View(new List<Menu>());
            }
            var json = await response.Content.ReadAsStringAsync();

            var menuList = JsonConvert.DeserializeObject<List<Menu>>(json);
            if (!string.IsNullOrEmpty(searchTitle))
            {
                menuList = menuList.Where(m =>m.DishName.Contains(searchTitle)).ToList();
            }
            //int numericSearch = 0;
            //    int.TryParse(searchTitle, out numericSearch);

            if (searchYear.HasValue)
            {
                menuList = menuList.Where(m => m.Price == searchYear.Value).ToList(); 
            }

            return View(menuList);
        }
       
        [HttpPost]
        public async Task<IActionResult> Create(Menu menu)
        {
            var token = HttpContext.Session.GetString("JwtToken");

            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonContent = JsonConvert.SerializeObject(menu);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"{_baseUri}api/Menu", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to create menu.");
            return View(menu);
        }
       
        [HttpPost]
        public async Task<IActionResult> Update(int id, Menu menu)
        {
            var token = HttpContext.Session.GetString("JwtToken");

            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonContent = JsonConvert.SerializeObject(menu);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"{_baseUri}api/Menu/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to update menu.");
            return View(menu);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
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

