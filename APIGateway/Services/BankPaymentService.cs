using System.Threading.Tasks;
using APIGateway.Helpers;
using APIGateway.Interfaces;
using APIGateway.Models;
using APIGateway.Models.DB;
using Microsoft.Extensions.Logging;

namespace APIGateway.Services
{
    public class BankPaymentService : IBankPayment
    {
        private readonly ILogger<BankPaymentService> _log;
        public BankPaymentService(ILogger<BankPaymentService> log)
        {
            _log = log;
        }

        public async Task<BankPaymentResponse> SendPayment(Payment payment)
        {
            return await Task.Run(() =>
            {
                _log.LogInformation("Mocking payment send to bank... If amount value is >50 it will fail.");
            
                if (payment.Amount > 50)
                    return new BankPaymentResponse
                    {
                        PaymentId = null,
                        Status = Constants.BankResponses.Failed
                    };

                return new BankPaymentResponse
                {
                    PaymentId = "id" + payment.Id + "am" + payment.Amount,
                    Status = Constants.BankResponses.Success
                };
            });
        }
    }
}
