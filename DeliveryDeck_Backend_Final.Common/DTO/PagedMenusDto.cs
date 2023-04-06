using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.DTO
{
    public class PagedMenusDto
    {
        public ICollection<MenuDto> Menus { get; set; } = new List<MenuDto>();
        public PageInfo PageInfo { get; set; }
    }
}
