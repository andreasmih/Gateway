using System.Threading.Tasks;
using APIGateway.Models;

namespace APIGateway.Interfaces
{
    public interface IPaymentValidationService
    {
        Task<bool> ValidatePaymentDetails(PaymentDetails payment);
    }
}
