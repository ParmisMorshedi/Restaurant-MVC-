using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantFrontend.Models;
using System.Text;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;



namespace RestaurantFrontend.Controllers
{
    public class ReservationController : Controller
    {
        private readonly HttpClient _client;
        private string _baseUri = "https://localhost:7071/";
        public ReservationController (HttpClient client) 
        {
            _client = client;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {

                ViewData["Title"] = "Book table";
                var response = await _client.GetAsync($"{_baseUri}api/Reservation");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var reservationList = JsonConvert.DeserializeObject<List<ReservationDTO>>(json);
                return View(reservationList);
            }
            catch (HttpRequestException ex)
            {
               
                ViewData["Error"] = "Error fetching reservations: " + ex.Message;
                return View(new List<Reservation>());
            }
        }
        public IActionResult Create() 
        {
            ViewData["Title"] = "New Reservation";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddReservation(ReservationDTO reservationDTO) 
        {
            if (!ModelState.IsValid)
            {
                return View(reservationDTO);
            }
            var json = JsonConvert.SerializeObject(reservationDTO);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{_baseUri}api/Reservation/AddReservation", content);
            return RedirectToAction("Index");
        }
       

    }
}
