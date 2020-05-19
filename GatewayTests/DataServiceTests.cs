using System;
using System.Threading.Tasks;
using APIGateway.Models;
using APIGateway.Models.DB;
using NUnit.Framework;

namespace GatewayTests
{
    public class DataServiceTests: TestBase
    {
        [Test]
        public async Task GetPaymentTestAsync()
        {
            //Arrange
            var initialPayment = new Payment
            {
                Id = 1,
                CardNumber = "1234652484747386",
                Status = "FAILED"
            };
            context.Payments.Add(initialPayment);
            context.SaveChanges();

            //Act
            var payment = await dataService.GetPayment(1);

            //Assert
            Assert.AreEqual(initialPayment.CardNumber, payment.CardNumber, $"Expected card number to be {initialPayment.CardNumber}, but it is {payment.CardNumber}");
            Assert.AreEqual(initialPayment.Status, payment.Status, $"Expected payment status to be {initialPayment.Status}, but it is {payment.Status}");

            Assert.IsNull(await dataService.GetPayment(2));
        }

        [Test]
        public async Task IsLegitimateMerchantTestAsync()
        {
            //Arrange
            var initialMer = new Merchant
            {
                Id = 100,
                Name = "Samsung",
            };
            context.Merchants.Add(initialMer);
            context.SaveChanges();
            
            //Act
            var merchOne = await dataService.IsLegitimateMerchant(100);
            var merchTwo = await dataService.IsLegitimateMerchant(200);

            //Assert
            Assert.AreEqual(true, merchOne, $"Expected merchant legitimacy to be true, but it is {merchOne}");
            Assert.AreEqual(false, merchTwo, $"Expected merchant legitimacy to be false, but it is {merchTwo}");
        }
        [Test]
        public void SavePaymentTest()
        {
            //Arrange
            var payment = new Payment
            {
                CardNumber = "6783968309687243",
                Cvv = "472",
                ExpiryDate = new DateTime(2022,10,12),

                Currency = "USD",
                Amount = 44.56m
            };
            //Act
            var id = dataService.SavePayment(payment);

            //Assert
            Assert.AreEqual(1, payment.Id, $"Expected id to be 1, but it is {payment.Id}");
        }
        [Test]
        public async Task ValidatePaymentTestAsync()
        {
            //Arrange
            var payment = new Payment
            {
                Id = 1,
                CardNumber = "6783968309687243",
                Cvv = "472",
                ExpiryDate = new DateTime(2022, 10, 12),

                Currency = "USD",
                Amount = 44.56m
            };
            context.Payments.Add(payment);
            context.SaveChanges();

            var bankResponse = new BankPaymentResponse
            {
                Status = "OK",
                PaymentId = "BankFakePaymentId"
            };

            //Act
            await dataService.ValidatePayment(1, bankResponse);

            //Assert
            Assert.AreEqual(payment.Status, bankResponse.Status, $"Expected status to be {payment.Status}, but it was {bankResponse.Status}");
            Assert.AreEqual(payment.BankUniqueId, bankResponse.PaymentId, $"Expected status to be {payment.BankUniqueId}, but it was {bankResponse.PaymentId}");
        }
    }
}