using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.DAL.Auth.Entities
{
    public class RefreshUserToken
    {
        [Key]
        public string Value { get; set; }
        public AppUser User { get; set; }
    }
}
