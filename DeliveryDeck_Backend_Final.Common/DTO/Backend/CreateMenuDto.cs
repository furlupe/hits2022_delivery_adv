using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.DTO.Backend
{
    public class CreateMenuDto
    {
        public string Name { get; set; }
        public List<Guid> Dishes { get; set; }
    }
}
