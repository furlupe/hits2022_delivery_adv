﻿using AdminPanel.Models;
using AutoMapper;
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
            return View("Index", _mapper.Map<UserListModel>(await _userService.GetUsers(page)));
        }
    }
}
