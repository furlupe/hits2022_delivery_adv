using DeliveryDeck_Backend_Final.Common.DTO.Auth;
using System.Security.Claims;

namespace DeliveryDeck_Backend_Final.Common.Interfaces.Auth
{
    public interface ITokenService
    {
        Token CreateAccessToken(ClaimsIdentity user);
        Token CreateRefreshToken();

    }
}
