﻿using DeliveryDeck_Backend_Final.Common.Enumerations;

namespace AdminPanel.Models
{
    public class StaffModel
    {
        public Guid Id { get; set; }
        public RoleType Role { get; set; }
    }
}