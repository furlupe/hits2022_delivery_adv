using AdminPanel.Models;
using AutoMapper;
using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.AdminPanel;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class UserController : Controller
    {
        private readonly IAdminUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IAdminUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            var users = await _userService.GetUsers(page);
            var response = _mapper.Map<UserListModel>(users);
            return View("Index",response);
        }

        public async Task<IActionResult> UsersPage(int page = 1)
        {
            return PartialView("UserListPartial", _mapper.Map<UserListModel>(await _userService.GetUsers(page)));
        }

        public async Task<IActionResult> UsersChoosePage(int page = 1)
        {
            var response = _mapper.Map<AvailableStaffListModel>(await _userService.GetAvailableStaff(page));
            ViewBag.PageInfo = response.PageInfo;
            return PartialView("UserChooseListPartial", response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateModel data)
        {
            await _userService.CreateUser(_mapper.Map<UserCreateDto>(data));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(Guid id, UserUpdateModel data)
        {
            await _userService.UpdateUser(id, _mapper.Map<UserUpdateDto>(data));
            return RedirectToAction("Details", new {id});
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.DeleteUser(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(Guid id)
        {
            return View("Details", _mapper.Map<UserModel>(await _userService.GetUserInfo(id)));
        }

        public async Task<IActionResult> BanUser(Guid id)
        {
            await _userService.BanUser(id);
            return RedirectToAction("Details", new {id});
        }
    }
}
