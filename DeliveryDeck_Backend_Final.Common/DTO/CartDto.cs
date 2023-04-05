using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.DTO
{
    public class CartDto
    {
        public ICollection<DishCartDto> Dishes { get; set; }
        public int Price { get; set; }
    }
}
