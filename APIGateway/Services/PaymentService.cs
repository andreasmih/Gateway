using System;
using System.Threading.Tasks;
using APIGateway.Interfaces;
using APIGateway.Models;
using APIGateway.Models.DB;

namespace APIGateway.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IDataService _dataService;
        private readonly IBankPayment _payment;
        public PaymentService(IBankPayment payment, IDataService dataService)
        {
            _dataService = dataService;
            _payment = payment;
        }

        public async Task<BankPaymentResponse> ForwardPayment(Payment payment)
        {
            var internalPaymentId = await _dataService.SavePayment(payment);

            BankPaymentResponse resp = await _payment.SendPayment(payment);
            if (resp?.Status == null) 
                throw new Exception("The bank payment failed");

            await _dataService.ValidatePayment(internalPaymentId, resp);
            return resp;
        }

        public Task<Payment> GetPayment(int id)
        {
            return _dataService.GetPayment(id);
        }
    }
}
