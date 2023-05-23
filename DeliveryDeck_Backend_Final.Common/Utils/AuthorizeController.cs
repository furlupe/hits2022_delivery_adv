using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DeliveryDeck_Backend_Final.Common.Utils
{
    public class AuthorizeController : ControllerBase
    {
        protected Guid UserId => Guid.Parse(User.FindFirst(ClaimTypes.Name).Value);
    }
}
