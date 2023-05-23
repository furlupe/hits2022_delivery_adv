using DeliveryDeck_Backend_Final.Common.DTO.Auth;
using DeliveryDeck_Backend_Final.Common.Interfaces.Auth;
using DeliveryDeck_Backend_Final.JWT.Classes;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DeliveryDeck_Backend_Final.Auth.BLL.Services
{
    internal class JwtTokenService : ITokenService
    {
        private readonly IKeyProvider _keyProvider;
        private readonly JwtConfig _jwtConfig;
        public JwtTokenService(IKeyProvider keyProvider, IOptions<JwtConfig> config)
        {
            _keyProvider = keyProvider;
            _jwtConfig = config.Value;
        }
        public Token CreateAccessToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;
            var expiry = now.AddMinutes(_jwtConfig.Lifetime);
            var jwt = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: expiry,
                signingCredentials: new SigningCredentials(_jwtConfig.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
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
