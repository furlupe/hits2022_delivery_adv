﻿using DeliveryDeck_Backend_Final.Common.DTO.Auth;

namespace DeliveryDeck_Backend_Final.Common.Interfaces.Auth
{
    public interface IAuthService
    {
        Task Register(UserRegistrationDto data);
        Task<TokenPairDto> Login(LoginCredentials credentials);
        Task Logout(Guid userId);
        Task<TokenPairDto> Refresh(string refreshToken);
    }
}
