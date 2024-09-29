
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
        private string baseUri = "https://localhost:7071/";
        public ReservationController(HttpClient client)
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
            var response = await _client.PostAsync($"{baseUri}api/Reservation/AddReservation", content);
            return RedirectToAction("Index");
        }
        // DELETE: Delete Reservation
        [HttpPost]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"{baseUri}api/Reservation/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                ViewData["Error"] = "Failed to delete reservation.";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                ViewData["Error"] = "Error deleting reservation: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _client.GetAsync($"{baseUri}api/Reservation/{id}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var reservation = JsonConvert.DeserializeObject<ReservationDTO>(json);
            return View(reservation);
        }

        //POST: Update Reservation
        [HttpPost]
        public async Task<IActionResult> UpdateReservation(ReservationDTO reservationDTO)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", reservationDTO);
            }

            try
            {
                var json = JsonConvert.SerializeObject(reservationDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync($"{baseUri}api/Reservation/{reservationDTO.ReservationId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = "Failed to update reservation.";
                    return View("Edit", reservationDTO);
                }
            }
            catch (HttpRequestException ex)
            {
                ViewData["Error"] = "Error updating reservation: " + ex.Message;
                return View("Edit", reservationDTO);
            }
        }
    }

