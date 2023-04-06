using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.DTO
{
    public class PagedDishesDto
    {
        public ICollection<DishShortDto> Dishes { get; set; } = new List<DishShortDto> ();
        public PageInfo PageInfo { get; set; }
    }
}
