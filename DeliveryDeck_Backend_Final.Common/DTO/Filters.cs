using DeliveryDeck_Backend_Final.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.DTO
{
    public class Filters
    {
        public ICollection<FoodCategory> Categories { get; set; }
        public string? Menu { get; set; }
        public bool? IsVegetarian { get; set; }
        public SortingType? SortBy { get; set; }
    }
}
