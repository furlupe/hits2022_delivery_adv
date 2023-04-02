using DeliveryDeck_Backend_Final.Common.DTO;

namespace DeliveryDeck_Backend_Final.Common.Interfaces
{
    public interface IAuthService
    {
        Task Register(UserRegistrationDto data);
        Task<TokenPairDto> Login(LoginCredentials credentials);
        Task Logout(Guid userId);
        Task<TokenPairDto> Refresh(string refreshToken);
        Task ChangePassword(Guid userId, ChangePasswordDto passwords);
    }
}
