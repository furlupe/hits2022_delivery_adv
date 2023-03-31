﻿using DeliveryDeck_Backend_Final.Common.DTO;
using System.Security.Claims;

namespace DeliveryDeck_Backend_Final.Common.Interfaces
{
    public interface ITokenService
    {
        Token CreateAccessToken(ClaimsIdentity user);
        Token CreateRefreshToken();

    }
}