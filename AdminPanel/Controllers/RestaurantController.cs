using AdminPanel.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class RestaurantController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return Redirect("https://www.youtube.com/watch?v=VZrDxD0Za9I&list=PLu4wnki9NI_8VmJ7Qz_byhKwCquXcy6u9");
        }

        [HttpPost]
        public IActionResult CreateRestaurant(CreateRestaurantModel model)
        {
            if(!ModelState.IsValid)
            {
                return View("Index");
            }


            return View("Index");
        }
    }
}
