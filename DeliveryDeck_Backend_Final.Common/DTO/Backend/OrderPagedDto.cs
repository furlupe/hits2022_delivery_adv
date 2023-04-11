﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.DTO.Backend
{
    public class OrderPagedDto
    {
        public ICollection<OrderShortDto> Orders { get; set; } = new List<OrderShortDto>();
        public PageInfo PageInfo { get; set; }
    }
}
