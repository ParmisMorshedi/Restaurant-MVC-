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
                // Return the view with validation errors if the model state is not valid
                return View(reservation);
            }
            try
            {
                var json = JsonConvert.SerializeObject(reservation);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

              
                var response = await _client.PostAsync($"{_baseUri}api/Reservation/Create", content);

                if (response.IsSuccessStatusCode)
                {
                    // Redirect to the Index action if the creation is successful
                    return RedirectToAction("Index");
                }
                else
                {
                    // Handle the error response if the creation failed
                    ModelState.AddModelError(string.Empty, "Server error. Please try again later.");
                    return View(reservation);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred: " + ex.Message);
                return View(reservation);
            }
        }
    }
       

    
}
