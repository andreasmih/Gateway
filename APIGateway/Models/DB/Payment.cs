using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Models.DB
{
    public class Payment
    {
        public int Id { get; set; }
        public string BankUniqueId { get; set; }
        public string Status { get; set; }

        public string CardNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Cvv { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public int MerchantId { get; set; }

        public virtual Merchant Merchant { get; set; }
    }

}
