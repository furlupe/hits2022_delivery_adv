﻿namespace DeliveryDeck_Backend_Final.Backend.DAL.Entities
{
    public class Cook
    {
        public Guid Id { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
