﻿using DeliveryDeck_Backend_Final.Common.Enumerations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryDeck_Backend_Final.Auth.DAL.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public ICollection<UserRole> Roles { get; set; }
        public override string Email { get; set; }
        public override string? UserName => Email;

        public Cook? Cook { get; set; }
        public Customer? Customer { get; set; }
        public Manager? Manager { get; set; }
        public Courier? Courier { get; set; }

        public bool IsBanned { get; set; } = false;

        [NotMapped]
        public List<RoleType> RoleTypes
        {
            get
            {
                return Roles.Select(x => x.Role.Type).ToList();
            }
        }
    }

}
