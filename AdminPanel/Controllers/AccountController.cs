using AdminPanel.Models;
using AutoMapper;
using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;
using DeliveryDeck_Backend_Final.Common.Interfaces.AdminPanel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAdminAuthService _authService;
        private readonly IMapper _mapper;
        public AccountController(IAdminAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(LoginModel credentials)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Wrong credentials");
            }
            await _authService.Login(_mapper.Map<LoginDto>(credentials));
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return RedirectToAction("Index");
        }

        public IActionResult AccessDenied()
        {
            return RedirectToAction("Index");
        }
    }
}
