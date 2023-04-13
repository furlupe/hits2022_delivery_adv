﻿using DeliveryDeck_Backend_Final.Common.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DeliveryDeck_Backend_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public abstract class AuthorizeController : ControllerBase
    {
        protected Guid UserId => Guid.Parse(User.FindFirst(ClaimTypes.Name).Value);
    }
}