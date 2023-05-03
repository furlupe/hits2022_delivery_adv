using AdminPanel.Models;
using AutoMapper;
using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;
using DeliveryDeck_Backend_Final.Common.Interfaces.AdminPanel;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly IAdminRestaurantService _restaurantService;
        private readonly IMapper _mapper;

        public RestaurantController(IAdminRestaurantService restaurantService, IMapper mapper)
        {
            _restaurantService = restaurantService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int page = 1, string? name = null)
        {
            ViewBag.Name = name;
            return View("Index", await _restaurantService.GetRestaurants(page, name));
        }

        public IActionResult About()
        {
            return Redirect("https://www.youtube.com/watch?v=VZrDxD0Za9I&list=PLu4wnki9NI_8VmJ7Qz_byhKwCquXcy6u9");
        }

        [HttpPost]
        public async Task<IActionResult> CreateRestaurant(RestaurantModel model)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            await _restaurantService.CreateRestaurant(_mapper.Map<RestaurantDto>(model));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _restaurantService.DeleteRestaurant(id);
            return RedirectToAction("Index");
        }
    }
}
