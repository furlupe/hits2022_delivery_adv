using AdminPanel.Models;
using AutoMapper;
using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.AdminPanel;
using Microsoft.AspNetCore.Mvc;
using static DeliveryDeck_Backend_Final.Common.Filters.RoleRequirementAuthorization;

namespace AdminPanel.Controllers
{
    [RoleRequirementAuthorization(RoleType.Admin)]
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
            return View("Index", _mapper.Map<RestaurantListModel>(await _restaurantService.GetRestaurants(page, name)));
        }

        public async Task<IActionResult> RestaurantsPage(int page = 1, string? name = null)
        {
            ViewBag.Name = name;
            return PartialView(
                "RestaurantListPartial",
                _mapper.Map<RestaurantListModel>(await _restaurantService.GetRestaurants(page, name))
            );
        }

        public IActionResult About()
        {
            return Redirect("https://www.youtube.com/watch?v=VZrDxD0Za9I&list=PLu4wnki9NI_8VmJ7Qz_byhKwCquXcy6u9");
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var restaurants = await _restaurantService.GetRestaurantInfo(id);
            var response = _mapper.Map<RestaurantModel>(restaurants);

            return View("Details", response);
        }

        public async Task<IActionResult> StaffPage(Guid id, int page = 1)
        {
            var restaurants = await _restaurantService.GetRestaurantInfo(id, page);
            var response = _mapper.Map<RestaurantModel>(restaurants);

            return PartialView("StaffListPartial", response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRestaurant(RestaurantCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            await _restaurantService.CreateRestaurant(_mapper.Map<RestaurantCreateDto>(model));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddStaffToRestaurant(Guid restaurantId, StaffModel data)
        {
            await _restaurantService.AddStaffToRestaurant(restaurantId, _mapper.Map<StaffDto>(data));
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> DismissStaff(Guid restaurantId, Guid staffId, RoleType fromRole)
        {
            await _restaurantService.DismissStaffFromRestaurant(restaurantId, staffId, fromRole);
            return RedirectToAction("Details", new { id = restaurantId });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _restaurantService.DeleteRestaurant(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(Guid id, RestaurantUpdateModel data)
        {
            if (ModelState.IsValid)
            {
                await _restaurantService.UpdateRestaurant(id, _mapper.Map<RestaurantUpdateDto>(data));
            }
            return RedirectToAction("Details", new { id });
        }
    }
}
