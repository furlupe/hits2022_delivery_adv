using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Backend.DAL.Entities
{
    public class Menu
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Dish> Dishes { get; set; }
        public Restaurant Restaurant { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
