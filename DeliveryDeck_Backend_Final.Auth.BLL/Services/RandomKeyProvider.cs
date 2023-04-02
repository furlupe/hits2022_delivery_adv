using DeliveryDeck_Backend_Final.Common.Interfaces;

namespace DeliveryDeck_Backend_Final.Auth.BLL.Services
{
    internal class RandomKeyProvider : IKeyProvider
    {
        private readonly string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
        public string CreateKey(int length)
        {
            Random random = new Random();
            string randomString = string.Empty;

            while (randomString.Length < length)
            {
                randomString += _alphabet[random.Next(_alphabet.Length)];
            }

            return randomString;
        }
    }

}
