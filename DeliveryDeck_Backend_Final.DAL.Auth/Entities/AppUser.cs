﻿using DeliveryDeck_Backend_Final.Common.Enumerations;
using Microsoft.AspNetCore.Identity;

namespace DeliveryDeck_Backend_Final.DAL.Auth.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public ICollection<UserRole> Roles { get; set; }
        public override string? UserName => Email;
    }

}
