using DeliveryDeck_Backend_Final.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.DTO.Backend
{
    public class OrderShortDto
    {
        public Guid Id { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime DeliveryTime { get; set; }
        public int Price { get; set; }
        public OrderStatus Status { get; set; }
        public string Address { get; set; }
    }
}
