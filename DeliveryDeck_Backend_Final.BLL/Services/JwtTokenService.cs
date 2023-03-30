using DeliveryDeck_Backend_Final.Common.DTO;
using DeliveryDeck_Backend_Final.Common.Interfaces;
using DeliveryDeck_Backend_Final.JWT.Classes;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DeliveryDeck_Backend_Final.BLL.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly IKeyProvider _keyProvider;
        public JwtTokenService(IKeyProvider keyProvider) 
        {
            _keyProvider = keyProvider;
        }
        public Token CreateAccessToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;
            var expiry = now.AddMinutes(JwtConfigurations.Lifetime);
            var jwt = new JwtSecurityToken(
                issuer: JwtConfigurations.Issuer,
                audience: JwtConfigurations.Audience,
            notBefore: now,
                claims: identity.Claims,
                expires: expiry,
                signingCredentials: new SigningCredentials(JwtConfigurations.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var enctoken = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new Token
            {
                Value = enctoken,
                Expiry = expiry.Ticks
            };

        }

        public Token CreateRefreshToken()
        {
            return new Token
            {
                Value = _keyProvider.CreateKey(26),
                Expiry = DateTime.UtcNow.AddDays(60).Ticks
            };
        }
    }
}
