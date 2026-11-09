using Microsoft.AspNetCore.Mvc;
using RaceManagementMVC.Models;
using RaceManagementMVC.Services;
using System.Text.Json;

namespace RaceManagementMVC.Controllers
{
    public class EventsController : Controller
    {
        private readonly apiService _apiService;

        public EventsController(apiService apiService)
        {
            _apiService = apiService;
        }

        // Display all events.
        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync("api/events");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Could not load events.";
                return View(new List<eventModel>());
            }

            string json = await response.Content.ReadAsStringAsync();

            var events = JsonSerializer.Deserialize<List<eventModel>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return View(events ?? new List<eventModel>());
        }

        // Display one event.
        public async Task<IActionResult> Details(int id)
        {
            var response = await _apiService.GetAsync($"api/events/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            string json = await response.Content.ReadAsStringAsync();

            var raceEvent = JsonSerializer.Deserialize<eventModel>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (raceEvent == null)
            {
                return NotFound();
            }

            return View(raceEvent);
        }

        // Display the create form.
        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("role") != "Organiser")
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        // Send the new event to the API.
        [HttpPost]
        public async Task<IActionResult> Create(createEventRequest request)
        {
            if (HttpContext.Session.GetString("role") != "Organiser")
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            request.eventName = request.eventName.Trim();
            request.description = request.description.Trim();
            request.venue = request.venue.Trim();
            request.eventType = request.eventType.Trim();

            var response = await _apiService.PostAsync(
                "api/events", request);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "The event could not be created. Please check your details.";
                return View(request);
            }

            TempData["Success"] = "Event created successfully.";

            return RedirectToAction("Index");
        }

        // Display the edit form.
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("role") != "Organiser")
            {
                return RedirectToAction("Login", "Auth");
            }

            var response = await _apiService.GetAsync($"api/events/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            string json = await response.Content.ReadAsStringAsync();

            var raceEvent = JsonSerializer.Deserialize<eventModel>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (raceEvent == null)
            {
                return NotFound();
            }

            var request = new updateEventRequest
            {
                eventName = raceEvent.eventName,
                description = raceEvent.description,
                eventDate = raceEvent.eventDate,
                startTime = raceEvent.startTime,
                venue = raceEvent.venue,
                distance = raceEvent.distance,
                eventType = raceEvent.eventType,
                maxParticipants = raceEvent.maxParticipants,
                status = raceEvent.status
            };

            return View(request);
        }

        // Update the event.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, updateEventRequest request)
        {
            if (HttpContext.Session.GetString("role") != "Organiser")
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var response = await _apiService.PutAsync(
                $"api/events/{id}", request);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "The event could not be updated.";
                return View(request);
            }

            TempData["Success"] = "Event updated successfully.";

            return RedirectToAction("Index");
        }

        // Delete an event.
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("role") != "Organiser")
            {
                return RedirectToAction("Login", "Auth");
            }

            var response = await _apiService.DeleteAsync(
                $"api/events/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "The event could not be deleted.";
            }
            else
            {
                TempData["Success"] = "Event deleted successfully.";
            }

            return RedirectToAction("Index");
        }
    }
}
