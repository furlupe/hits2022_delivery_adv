using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.DTO.AdminPanel
{
    public class RestaurantDto
    {
        public string Name { get; set; }
        public List<Guid> Managers { get; set; }
        public List<Guid> Cooks { get; set; }
    }
}
