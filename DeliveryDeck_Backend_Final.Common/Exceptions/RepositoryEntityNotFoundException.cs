namespace DeliveryDeck_Backend_Final.Common.Exceptions
{

    [Serializable]
    public class RepositoryEntityNotFoundException : Exception
    {
        public object? Optional { get; set; }
        public RepositoryEntityNotFoundException(object? optional = default)
        {
            Optional = optional;
        }

        public RepositoryEntityNotFoundException(string? message, object? optional = default) : base(message)
        {
            Optional = optional;
        }

        public RepositoryEntityNotFoundException(string? message, Exception? innerException, object? optional = default) : base(message, innerException)
        {
            Optional = optional;
        }
    }
}
