using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RannaTaskTwoPanel.Controllers
{
    public class AccountController : Controller
    {
        private readonly string apiBase = "https://localhost:7014/api/";

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(apiBase);

            var response = await client.PostAsJsonAsync("Auth/login", new { username, password });
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<LoginResponse>();
                HttpContext.Session.SetString("Token", json.AccessToken);
                return RedirectToAction("Index", "Product");
            }
            ModelState.AddModelError("", "Login failed");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login");
        }

        private class LoginResponse
        {
            public string AccessToken { get; set; }
            public string Expiration { get; set; }
        }
    }
} 