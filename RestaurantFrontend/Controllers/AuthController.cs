using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using RestaurantFrontend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantFrontend.Controllers
{

    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;
        //private readonly ApiService _apiService;

        //private string _baseUri = "https://localhost:7071/";
        public AuthController(HttpClient client)
        {
            _httpClient = client;
            _httpClient.BaseAddress = new Uri("https://localhost:7071/");
        }
        
        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]        
        public async Task<IActionResult> Login(LoginViewModel loginUser)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginUser);

            if (!response.IsSuccessStatusCode)
            {
                return View(loginUser);
            }

            //try
            //{
                //var token = await _apiService.LoginAsync(loginUser);
                //if (!string.IsNullOrEmpty(token))
                //{
                //    HttpContext.Session.SetString("JWTToken", token); // Spara token
                //    return RedirectToAction("Index", "Home");
                //}
            //}
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            //}
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenResponse>(jsonResponse);

            var handlar = new JwtSecurityTokenHandler();
            var jwtToken = handlar.ReadJwtToken(token.Token);

            var claims = jwtToken.Claims.ToList();

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal= new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc= jwtToken.ValidTo
            });
            HttpContext.Response.Cookies.Append("jwtToken", token.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, //Just Https
                SameSite= SameSiteMode.Strict,
                Expires = jwtToken.ValidTo
            });

            //ModelState.AddModelError(string.Empty, "Login failed. Please check your credentials.");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout() 
        {
            
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Cookies.Delete("jwtToken");

            return RedirectToAction("Login", "Auth");
        }
    }
}




//[HttpGet]
//public IActionResult Register()
//{
//    return View();
//}

