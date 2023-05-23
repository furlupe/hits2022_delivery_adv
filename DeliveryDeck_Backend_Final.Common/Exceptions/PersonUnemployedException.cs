namespace DeliveryDeck_Backend_Final.Common.Exceptions
{
    public class PersonUnemployedException : Exception
    {
        public PersonUnemployedException()
        {
        }

        public PersonUnemployedException(string? message) : base(message)
        {
        }

        public PersonUnemployedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
