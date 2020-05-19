using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIGateway.Interfaces;
using APIGateway.Models;
using APIGateway.Models.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace APIGateway.Services
{
    public class DataService : IDataService
    {
        private readonly GatewayContext _context;
        private readonly ILogger<DataService> _log;
        public DataService(GatewayContext context, ILogger<DataService> log)
        {
            _context = context;
            _log = log;
        }

        public async Task<Merchant> AuthenticateMerchant(string username, string password)
        {
            var user = await _context.Merchants.Where(o => o.Username == username).FirstOrDefaultAsync();
            if (user == null)
            {
                _log.LogError($"User with username {username} could not be found!");
                return null;
            }

            var hashedPass = HashPassword(password, user.Salt);
            if (user.Password == hashedPass)
                return user;

            _log.LogError($"User with username {username} has put a wrong password in!");
            return null;
        }

        public async Task<Payment> GetPayment(int paymentId)
        {
            return await _context.Payments.FindAsync(paymentId);
        }

        public async Task<bool> IsLegitimateMerchant(int merchantId)
        {
            var merchant = await _context.Merchants.FindAsync(merchantId);
            return merchant != null;
        }

        public async Task<int> SavePayment(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            _log.LogInformation($"Payment {payment.Id} saved!");
            return payment.Id;
        }

        public async Task<int> ValidatePayment(int paymentId, BankPaymentResponse response)
        {
            _log.LogInformation($"Validating payment {paymentId}");
            var payment = await _context.Payments.FindAsync(paymentId);
            payment.Status = response.Status;
            payment.BankUniqueId = response.PaymentId;

            return await _context.SaveChangesAsync();
        }

        private string HashPassword(string password, string salt)
        {

            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password, Encoding.ASCII.GetBytes(salt), KeyDerivationPrf.HMACSHA256, 1000, 256/8));
        }
    }
}
