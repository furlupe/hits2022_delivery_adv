using AdminPanel.Models;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using Microsoft.AspNetCore.Mvc;
using static DeliveryDeck_Backend_Final.Common.Filters.RoleRequirementAuthorization;

namespace AdminPanel.Controllers
{
    [RoleRequirementAuthorization(RoleType.Admin)]
    public class HomeController : Controller
    {

        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return Redirect("https://www.youtube.com/watch?v=VZrDxD0Za9I");
        }

        public IActionResult Error(ErrorModel error)
        {
            return View("~/Views/Shared/Error.cshtml", error);
        }
    }
}