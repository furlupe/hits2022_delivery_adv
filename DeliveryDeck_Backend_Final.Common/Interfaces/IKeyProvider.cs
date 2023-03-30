using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.Interfaces
{
    public interface IKeyProvider
    {
        string CreateKey(int length);
    }
}
