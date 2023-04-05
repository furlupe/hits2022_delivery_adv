﻿using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace DeliveryDeck_Backend_Final.Backend.DAL.Entities
{
    public class Dish
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public bool IsVegeterian { get; set; }
        public Uri Photo { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public FoodCategory Category { get; set; }
    }
}
