﻿namespace DeliveryDeck_Backend_Final.Common.DTO.Auth
{
    public class Token
    {
        public string Value { get; set; }
        public long Expiry { get; set; }
    }
}
