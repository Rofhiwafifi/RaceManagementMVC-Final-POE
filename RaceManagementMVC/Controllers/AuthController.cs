using Microsoft.AspNetCore.Mvc;
using RaceManagementMVC.Models;
using RaceManagementMVC.Services;
using System.Text.Json;

namespace RaceManagementMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly apiService _apiService;
        public AuthController(apiService apiService) => _apiService = apiService;

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(registerRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            request.firstName = request.firstName.Trim();
            request.lastName = request.lastName.Trim();
            request.email = request.email.Trim().ToLowerInvariant();

            HttpResponseMessage response;
            try
            {
                // IMPORTANT: change this route only if your Part 2 API uses a different registration route.
                response = await _apiService.PostAsync("api/auth/register", request);
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Could not connect to the API. Make sure Part 2 is running.";
                return View(request);
            }

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                ViewBag.Error = string.IsNullOrWhiteSpace(body)
                    ? "Registration failed. Please check your details."
                    : body;
                return View(request);
            }

            TempData["Success"] = "Registration successful! Please log in.";
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(loginRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            request.email = request.email.Trim().ToLowerInvariant();

            HttpResponseMessage response;
            try
            {
                response = await _apiService.PostAsync("api/auth/login", request);
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Could not connect to the API. Make sure Part 2 is running.";
                return View(request);
            }

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid email or password.";
                return View(request);
            }

            var json = await response.Content.ReadAsStringAsync();
            var login = JsonSerializer.Deserialize<loginResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (login?.user == null || string.IsNullOrWhiteSpace(login.token))
            {
                ViewBag.Error = "The API returned an invalid login response.";
                return View(request);
            }

            HttpContext.Session.Clear();
            HttpContext.Session.SetString("token", login.token);
            HttpContext.Session.SetString("role", login.user.role ?? "");
            HttpContext.Session.SetString("userId", login.user.userId.ToString());
            HttpContext.Session.SetString("name", login.user.name ?? "");
            HttpContext.Session.SetString("surname", login.user.surname ?? "");
            HttpContext.Session.SetString("email", login.user.email ?? "");

            if (login.user.role == "Organiser")
                return RedirectToAction("Index", "Organiser");

            if (login.user.role == "Participant")
                return RedirectToAction("Index", "Participant");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
    }
}
