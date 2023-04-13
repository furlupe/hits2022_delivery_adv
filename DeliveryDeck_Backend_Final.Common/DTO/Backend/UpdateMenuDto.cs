using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.DTO.Backend
{
    public class UpdateMenuDto
    {
        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
