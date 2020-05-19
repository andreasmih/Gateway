using System.Threading.Tasks;
using APIGateway.Models;
using APIGateway.Models.DB;

namespace APIGateway.Interfaces
{
    public interface IPaymentService
    {
        Task<BankPaymentResponse> ForwardPayment(Payment payment);
        Task<Payment> GetPayment(int id);
    }
}
