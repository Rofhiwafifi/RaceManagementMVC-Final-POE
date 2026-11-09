using Microsoft.AspNetCore.Mvc;
using RaceManagementMVC.Models;
using RaceManagementMVC.Services;
using System.Text.Json;

namespace RaceManagementMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly apiService _apiService;

        public CategoriesController(apiService apiService)
        {
            _apiService = apiService;
        }

        // Display all categories.
        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync("api/categories");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Could not load categories.";
                return View(new List<categoryModel>());
            }

            string json = await response.Content.ReadAsStringAsync();

            var categories = JsonSerializer.Deserialize<List<categoryModel>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return View(categories ?? new List<categoryModel>());
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

        // Send a new category to the API.
        [HttpPost]
        public async Task<IActionResult> Create(createCategoryRequest request)
        {
            if (HttpContext.Session.GetString("role") != "Organiser")
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            request.categoryName = request.categoryName.Trim();
            request.description = request.description.Trim();

            var response = await _apiService.PostAsync(
                "api/categories", request);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "The category could not be created.";
                return View(request);
            }

            TempData["Success"] = "Category created successfully.";

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

            var response = await _apiService.GetAsync(
                $"api/categories/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            string json = await response.Content.ReadAsStringAsync();

            var category = JsonSerializer.Deserialize<categoryModel>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (category == null)
            {
                return NotFound();
            }

            var request = new createCategoryRequest
            {
                categoryName = category.categoryName,
                description = category.description
            };

            ViewBag.CategoryId = id;

            return View(request);
        }

        // Update a category.
        [HttpPost]
        public async Task<IActionResult> Edit(
            int id, createCategoryRequest request)
        {
            if (HttpContext.Session.GetString("role") != "Organiser")
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.CategoryId = id;
                return View(request);
            }

            var response = await _apiService.PutAsync(
                $"api/categories/{id}", request);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "The category could not be updated.";
                ViewBag.CategoryId = id;
                return View(request);
            }

            TempData["Success"] = "Category updated successfully.";

            return RedirectToAction("Index");
        }

        // Delete a category.
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("role") != "Organiser")
            {
                return RedirectToAction("Login", "Auth");
            }

            var response = await _apiService.DeleteAsync(
                $"api/categories/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "The category could not be deleted.";
            }
            else
            {
                TempData["Success"] = "Category deleted successfully.";
            }

            return RedirectToAction("Index");
        }
    }
}