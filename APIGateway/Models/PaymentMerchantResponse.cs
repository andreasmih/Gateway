using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Models
{
    public class PaymentMerchantResponse
    {
        public int Id { get; set; }
        public string BankReference { get; set; }
        public string Status { get; set; }
    }
}
