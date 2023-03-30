using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.DTO
{
    public class TokenPairDto
    {
        public Token AccessToken { get; set; }
        public Token RefreshToken { get; set; }
    }
}
