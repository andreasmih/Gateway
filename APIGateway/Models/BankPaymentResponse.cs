using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Models
{
    public class BankPaymentResponse
    {
        public string Status { get; set; }
        public string PaymentId { get; set; }
    }
}
