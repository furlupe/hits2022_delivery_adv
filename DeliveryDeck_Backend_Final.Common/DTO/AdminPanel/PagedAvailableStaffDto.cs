using DeliveryDeck_Backend_Final.Common.DTO.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.DTO.AdminPanel
{
    public class PagedAvailableStaffDto
    {
        public List<AvailableStaffDto> Users { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
