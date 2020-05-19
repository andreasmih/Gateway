using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGateway.Interfaces;
using APIGateway.Models;

namespace APIGateway.Services
{
    public class PaymentValidationService : IPaymentValidationService
    {
        private readonly IDataService _dataService;

        public PaymentValidationService(IDataService dataService)
        {
            _dataService = dataService;
        }
        public async Task<bool> ValidatePaymentDetails(PaymentDetails payment)
        {
            if (payment.CardNumber == null ||
                payment.Cvv == null ||
                payment.CardNumber == null ||
                payment.ExpiryDate == null||
                payment.Amount <= 0 ||
                payment.Currency == null ||
                !await _dataService.IsLegitimateMerchant(payment.MerchantId))
                return false;

            return true;
        }
    }
}
