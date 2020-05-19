using System;

namespace APIGateway.Models
{ 
    public class PaymentEnquiryResponse
    {
        public string MaskedCardNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Cvv { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; } 
        public string Currency { get; set; }
    }
}
