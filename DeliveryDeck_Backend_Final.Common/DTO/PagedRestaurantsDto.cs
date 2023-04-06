using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.DTO
{
    public class PagedRestaurantsDto
    {
        public ICollection<RestaurantShortDto> Restaurants { get; set; } = new List<RestaurantShortDto>();
        public PageInfo PageInfo { get; set; }
    }
}
