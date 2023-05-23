using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DeliveryDeck_Backend_Final.JWT.Classes
{
    public class JwtConfig
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Lifetime { get; set; }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
            => new(Encoding.ASCII.GetBytes(Key));
    }
}
