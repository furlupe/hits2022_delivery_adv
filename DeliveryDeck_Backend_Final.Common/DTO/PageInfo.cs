using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.DTO
{
    public class PageInfo
    {
        public int CurrentPage { get; set; }
        public int Pages { get; set; }
        public int PageSize { get; set; }
    }
}
