using Microsoft.AspNetCore.Mvc;
using RaceManagementMVC.Models;
using RaceManagementMVC.Services;
using System.Text.Json;

namespace RaceManagementMVC.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly apiService _apiService;
        public ParticipantController(apiService apiService) => _apiService = apiService;

        public async Task<IActionResult> Index(string? search, string? eventType, decimal? maxDistance)
        {
            if (HttpContext.Session.GetString("role") != "Participant")
                return RedirectToAction("Login", "Auth");

            var response = await _apiService.GetAsync("api/events");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Could not load upcoming events.";
                return View(new List<eventModel>());
            }

            var events = JsonSerializer.Deserialize<List<eventModel>>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();

            var today = DateTime.Today;
            events = events.Where(e => e.eventDate.Date >= today).ToList();

            if (!string.IsNullOrWhiteSpace(search))
                events = events.Where(e =>
                    (e.eventName ?? "").Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    (e.venue ?? "").Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(eventType))
                events = events.Where(e => e.eventType.Equals(eventType, StringComparison.OrdinalIgnoreCase)).ToList();

            if (maxDistance.HasValue)
                events = events.Where(e => e.distance <= maxDistance.Value).ToList();

            ViewBag.Search = search;
            ViewBag.EventType = eventType;
            ViewBag.MaxDistance = maxDistance;
            return View(events.OrderBy(e => e.eventDate).ToList());
        }
    }
}
