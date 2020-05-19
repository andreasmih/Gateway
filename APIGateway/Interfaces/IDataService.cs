using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGateway.Models;
using APIGateway.Models.DB;

namespace APIGateway.Interfaces
{
    public interface IDataService
    {
        Task<int> SavePayment(Payment payment);
        Task<int> ValidatePayment(int paymentId, BankPaymentResponse response);
        Task<Payment> GetPayment(int paymentId);
        Task<bool> IsLegitimateMerchant(int merchantId);
        Task<Merchant> AuthenticateMerchant(string username, string password);
    }
}
