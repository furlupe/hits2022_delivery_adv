using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.DTO
{
    public class RefreshDto
    {
        [Required]
        public string Value { get; set; }
    }

}
