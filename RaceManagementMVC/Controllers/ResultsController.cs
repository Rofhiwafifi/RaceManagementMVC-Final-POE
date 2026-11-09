using Microsoft.AspNetCore.Mvc;
using RaceManagementMVC.Models;
using RaceManagementMVC.Services;
using System.Text.Json;

namespace RaceManagementMVC.Controllers
{
    public class ResultsController : Controller
    {
        private readonly apiService _apiService;
        private readonly JsonSerializerOptions _jsonOptions =
            new() { PropertyNameCaseInsensitive = true };

        public ResultsController(apiService apiService) => _apiService = apiService;

        // Organiser: event selection plus results. No eventId means show selector,
        // instead of returning HTTP 400.
        public async Task<IActionResult> Index(int? eventId)
        {
            if (HttpContext.Session.GetString("role") != "Organiser")
                return RedirectToAction("Login", "Auth");

            var vm = new OrganiserEventSelectionViewModel { SelectedEventId = eventId };
            var eventsResponse = await _apiService.GetAsync("api/events");

            if (!eventsResponse.IsSuccessStatusCode)
            {
                ViewBag.Error = "Could not load events.";
                return View(vm);
            }

            vm.Events = JsonSerializer.Deserialize<List<eventModel>>(
                await eventsResponse.Content.ReadAsStringAsync(), _jsonOptions) ?? new();

            if (eventId.HasValue && eventId.Value > 0)
            {
                var response = await _apiService.GetAsync(
                    $"api/events/{eventId.Value}/results");

                if (response.IsSuccessStatusCode)
                    vm.Results = JsonSerializer.Deserialize<List<resultModel>>(
                        await response.Content.ReadAsStringAsync(), _jsonOptions) ?? new();
                else
                    ViewBag.Error = "Could not load results for this event.";
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create(int eventId, int enrolmentId)
        {
            if (HttpContext.Session.GetString("role") != "Organiser")
                return RedirectToAction("Login", "Auth");
            if (eventId <= 0 || enrolmentId <= 0) return BadRequest();

            ViewBag.EventId = eventId;
            return View(new createResultRequest
            {
                enrolmentId = enrolmentId,
                resultStatus = "Pending"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int eventId, createResultRequest request)
        {
            if (HttpContext.Session.GetString("role") != "Organiser")
                return RedirectToAction("Login", "Auth");
            if (eventId <= 0) return BadRequest();

            request.resultStatus = "Pending";

            if (!ModelState.IsValid)
            {
                ViewBag.EventId = eventId;
                return View(request);
            }

            var response = await _apiService.PostAsync(
                $"api/events/{eventId}/results", request);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Result could not be saved. Check that the API accepts the supplied enrolment ID.";
                ViewBag.EventId = eventId;
                return View(request);
            }

            TempData["Success"] = "Result saved as Pending. Publish it when ready.";
            return RedirectToAction(nameof(Index), new { eventId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Publish(int id, int eventId)
        {
            if (HttpContext.Session.GetString("role") != "Organiser")
                return RedirectToAction("Login", "Auth");
            if (id <= 0 || eventId <= 0) return BadRequest();

            var response = await _apiService.PutAsync(
                $"api/results/{id}", new { resultStatus = "Published" });

            TempData[response.IsSuccessStatusCode ? "Success" : "Error"] =
                response.IsSuccessStatusCode
                    ? "Result published."
                    : "Could not publish result. Check the API route and permissions.";

            return RedirectToAction(nameof(Index), new { eventId });
        }

        public async Task<IActionResult> MyResults()
        {
            if (HttpContext.Session.GetString("role") != "Participant")
                return RedirectToAction("Login", "Auth");

            var response = await _apiService.GetAsync("api/users/me/results");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Could not load your results.";
                return View(new List<resultModel>());
            }

            return View(JsonSerializer.Deserialize<List<resultModel>>(
                await response.Content.ReadAsStringAsync(), _jsonOptions) ?? new());
        }
    }
}
