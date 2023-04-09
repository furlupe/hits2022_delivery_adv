﻿using DeliveryDeck_Backend_Final.Common.DTO;
using DeliveryDeck_Backend_Final.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryDeck_Backend_Final.Controllers
{
    [Route("api/dishes")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;
        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpGet("{dishId}")]
        public async Task<DishDto> GetDish(Guid dishId)
        {
            return await _dishService.GetDishById(dishId);
        }
    }
}