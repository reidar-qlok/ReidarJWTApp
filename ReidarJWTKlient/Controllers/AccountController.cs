using Microsoft.AspNetCore.Mvc;
using ReidarJWTKlient.Models;
using System.Text;
using System.Text.Json;

namespace ReidarJWTKlient.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("http://localhost:7162/auth/login", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var token = JsonSerializer.Deserialize<AuthResponse>(responseContent).Token;

                    // Här kan du spara token i cookies eller session för att använda senare
                    HttpContext.Session.SetString("JWTToken", token);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Ogiltiga inloggningsuppgifter.");
            }

            return View(model);
        }
    }
}

