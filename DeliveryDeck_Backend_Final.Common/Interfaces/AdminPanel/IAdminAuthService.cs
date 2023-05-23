using DeliveryDeck_Backend_Final.Common.DTO.AdminPanel;

namespace DeliveryDeck_Backend_Final.Common.Interfaces.AdminPanel
{
    public interface IAdminAuthService
    {
        Task Login(LoginDto credentials);
        Task Logout();
    }
}
