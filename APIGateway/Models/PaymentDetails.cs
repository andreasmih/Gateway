﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace APIGateway.Models
{
    public class PaymentDetails
    {
        [JsonPropertyName("card_number")]
        public string CardNumber { get; set; }
        [JsonPropertyName("expiry_date")]
        public DateTime? ExpiryDate { get; set; }
        public string Cvv { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; }
        [JsonPropertyName("merchant_id")]
        public int MerchantId { get; set; }
    }
}
