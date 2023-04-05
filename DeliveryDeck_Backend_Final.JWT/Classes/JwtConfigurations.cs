using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DeliveryDeck_Backend_Final.JWT.Classes
{
    public static class JwtConfigurations
    {
        private const string Key = "obamahamburgersussyballs";
        public const string Issuer = "furlupe";
        public const string Audience = "not furlupe";
        public const int Lifetime = 60;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
            => new(Encoding.ASCII.GetBytes(Key));
    }
}
