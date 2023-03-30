using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.DTO
{
    public class Token
    {
        public string Value { get; set; }
        public long Expiry { get; set; }
    }
}
