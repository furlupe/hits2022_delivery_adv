using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDeck_Backend_Final.Common.Exceptions
{
    [Serializable]
    public class RepositoryEntityAlreadyExistsException : Exception
    {
        public RepositoryEntityAlreadyExistsException()
        {
        }

        public RepositoryEntityAlreadyExistsException(string? message) : base(message)
        {
        }

        public RepositoryEntityAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
