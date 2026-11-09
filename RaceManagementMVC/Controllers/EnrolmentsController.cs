using Microsoft.AspNetCore.Mvc;
using RaceManagementMVC.Models;
using RaceManagementMVC.Services;
using System.Text.Json;

namespace RaceManagementMVC.Controllers
{
    public class EnrolmentsController : Controller
    {
        private readonly apiService _apiService;
        private readonly JsonSerializerOptions _jsonOptions =
            new() { PropertyNameCaseInsensitive = true };

        public EnrolmentsController(apiService apiService) => _apiService = apiService;

        // Organiser page: choose an event first. This prevents the previous HTTP 400
        // caused by eventId being omitted from the dashboard link.
        public async Task<IActionResult> Index(int? eventId)
        {
            if (HttpContext.Session.GetString("role") != "Organiser")
                return RedirectToAction("Login", "Auth");

            var vm = new OrganiserEventSelectionViewModel { SelectedEventId = eventId };

            var eventsResponse = await _apiService.GetAsync("api/events");
            if (!eventsResponse.IsSuccessStatusCode)
            {
                ViewBag.Error = "Could not load events from the API.";
                return View(vm);
            }

            vm.Events = JsonSerializer.Deserialize<List<eventModel>>(
                await eventsResponse.Content.ReadAsStringAsync(), _jsonOptions) ?? new();

            if (eventId.HasValue && eventId.Value > 0)
            {
                var response = await _apiService.GetAsync(
                    $"api/events/{eventId.Value}/enrolments");

                if (response.IsSuccessStatusCode)
                {
                    vm.Enrolments = JsonSerializer.Deserialize<List<enrolmentModel>>(
                        await response.Content.ReadAsStringAsync(), _jsonOptions) ?? new();
                }
                else
                {
                    ViewBag.Error = "Could not load enrolments for the selected event.";
                }
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Enrol(int eventId)
        {
            if (HttpContext.Session.GetString("role") != "Participant")
                return RedirectToAction("Login", "Auth");

            if (eventId <= 0) return BadRequest();

            var response = await _apiService.GetAsync("api/categories");
            ViewBag.Categories = response.IsSuccessStatusCode
                ? JsonSerializer.Deserialize<List<categoryModel>>(
                    await response.Content.ReadAsStringAsync(), _jsonOptions) ?? new()
                : new List<categoryModel>();

            ViewBag.EventId = eventId;
            if (!response.IsSuccessStatusCode) ViewBag.Error = "Could not load categories.";
            return View(new createEnrolmentRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enrol(int eventId, createEnrolmentRequest request)
        {
            if (HttpContext.Session.GetString("role") != "Participant")
                return RedirectToAction("Login", "Auth");

            if (eventId <= 0) return BadRequest();

            if (!ModelState.IsValid || request.categoryId <= 0)
            {
                await ReloadCategories(eventId);
                return View(request);
            }

            var response = await _apiService.PostAsync(
                $"api/events/{eventId}/enrolments", request);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Enrolment failed. You may already be enrolled or the category may be unavailable.";
                await ReloadCategories(eventId);
                return View(request);
            }

            TempData["Success"] = "You're enrolled! See you at the starting line 🏁";
            return RedirectToAction(nameof(MyEnrolments));
        }

        private async Task ReloadCategories(int eventId)
        {
            var response = await _apiService.GetAsync("api/categories");
            ViewBag.Categories = response.IsSuccessStatusCode
                ? JsonSerializer.Deserialize<List<categoryModel>>(
                    await response.Content.ReadAsStringAsync(), _jsonOptions) ?? new()
                : new List<categoryModel>();
            ViewBag.EventId = eventId;
        }

        public async Task<IActionResult> MyEnrolments()
        {
            if (HttpContext.Session.GetString("role") != "Participant")
                return RedirectToAction("Login", "Auth");

            var response = await _apiService.GetAsync("api/users/me/enrolments");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Could not load your enrolments.";
                return View(new List<enrolmentModel>());
            }

            return View(JsonSerializer.Deserialize<List<enrolmentModel>>(
                await response.Content.ReadAsStringAsync(), _jsonOptions) ?? new());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int eventId)
        {
            if (HttpContext.Session.GetString("role") != "Participant")
                return RedirectToAction("Login", "Auth");

            var response = await _apiService.DeleteAsync(
                $"api/events/{eventId}/enrolments/me");

            TempData[response.IsSuccessStatusCode ? "Success" : "Error"] =
                response.IsSuccessStatusCode
                    ? "Your enrolment has been cancelled."
                    : "Your enrolment could not be cancelled.";

            return RedirectToAction(nameof(MyEnrolments));
        }
    }
}
