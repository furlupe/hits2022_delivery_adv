using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.Exceptions
{
    public class OrderUnableToChangeStatusException : Exception
    {
        public OrderUnableToChangeStatusException()
        {
        }

        public OrderUnableToChangeStatusException(string? message) : base(message)
        {
        }

        public OrderUnableToChangeStatusException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
