using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DeliveryDeck_Backend_Final.Common.Exceptions
{
    public class IdentityException : BadHttpRequestException
    {
        public IEnumerable<IdentityError> Errors { get; set; }
        public IdentityException(string message) : base(message)
        {
        }

        public IdentityException(string message, int statusCode) : base(message, statusCode)
        {
        }

        public IdentityException(string message, int statusCode, IEnumerable<IdentityError> errors) : base(message, statusCode)
        {
            Errors = errors;
        }
    }
}
