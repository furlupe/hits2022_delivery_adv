using DeliveryDeck_Backend_Final.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.DTO
{
    public class DishCartDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public bool IsVegeterian { get; set; }
        public Uri Photo { get; set; }
        public FoodCategory Category;
        public int Amount { get; set; }
    }
}
