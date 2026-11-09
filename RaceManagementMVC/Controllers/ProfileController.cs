using Microsoft.AspNetCore.Mvc;
using RaceManagementMVC.Models;
using RaceManagementMVC.Services;
using System.Text.Json;

namespace RaceManagementMVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly apiService _apiService;

        public ProfileController(apiService apiService)
        {
            _apiService = apiService;
        }

        private bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(
                HttpContext.Session.GetString("token"));
        }

        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var response = await _apiService.GetAsync("api/users/me");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Could not load your profile.";
                return View(new profileModel());
            }

            string json = await response.Content.ReadAsStringAsync();

            var profile = JsonSerializer.Deserialize<profileModel>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Update(updateProfileRequest request)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please enter valid profile details.";
                return RedirectToAction("Index");
            }

            var response = await _apiService.PutAsync(
                "api/users/me", request);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Your profile could not be updated.";
                return RedirectToAction("Index");
            }

            HttpContext.Session.SetString("name", request.name);
            HttpContext.Session.SetString("surname", request.surname);
            HttpContext.Session.SetString("email", request.email);

            TempData["Success"] = "Your profile was updated successfully.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UploadPicture(IFormFile profilePicture)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (profilePicture == null || profilePicture.Length == 0)
            {
                TempData["Error"] = "Please select a picture.";
                return RedirectToAction("Index");
            }

            if (!profilePicture.ContentType.StartsWith("image/"))
            {
                TempData["Error"] = "Please upload an image file.";
                return RedirectToAction("Index");
            }

            var response = await _apiService.PostFileAsync(
                "api/users/me/profile-picture",
                profilePicture,
                "profilePicture");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "The profile picture could not be uploaded.";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Profile picture uploaded successfully.";

            return RedirectToAction("Index");
        }
    }
}
