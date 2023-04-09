using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.DTO
{
    public class PagedDishesDto
    {
        public ICollection<DishDto> Dishes { get; set; } = new List<DishDto> ();
        public PageInfo PageInfo { get; set; }
    }
}
