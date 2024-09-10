using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantFrontend.Models;
using System.Text;
using System.Net.Http;


namespace RestaurantFrontend.Controllers
{
    public class ReservationController : Controller
    {
        private readonly HttpClient _client;
        private string baseUri = "https://localhost:7071/";
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
                var response = await _client.GetAsync($"{baseUri}api/Reservation");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var reservationList = JsonConvert.DeserializeObject<List<Reservation>>(json);
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
        public async Task<IActionResult> Create(Reservation reservation) 
        {

            if (!ModelState.IsValid)
            {
                return View(reservation);
            }
            var json = JsonConvert.SerializeObject(reservation);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{baseUri}api/Reservation/AddReservation", content);
            return RedirectToAction("Index");
        }
       

    }
}
