using Microsoft.AspNetCore.Mvc;

namespace RaceManagementMVC.Controllers
{
    public class OrganiserController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("role") != "Organiser")
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }
    }
}
