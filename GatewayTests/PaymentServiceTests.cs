using System;
using System.Threading.Tasks;
using APIGateway.Interfaces;
using APIGateway.Models;
using APIGateway.Models.DB;
using APIGateway.Services;
using Moq;
using NUnit.Framework;

namespace GatewayTests
{
    public class PaymentServiceTests : TestBase
    {
        [Test]
        public async Task ForwardPaymentResponseTestAsync()
        {
            //Arrange
            var initialPayment = new Payment
            {
                CardNumber = "6783968309687243",
                Cvv = "472",
                ExpiryDate = new DateTime(2022, 10, 12),

                Currency = "USD",
                Amount = 44.56m
            };

            //Act
            var bankResponse = await paymentService.ForwardPayment(initialPayment);

            //Assert
            Assert.AreEqual("OK", bankResponse.Status, $"Expected card number to be OK, but it is {bankResponse.Status}");
            Assert.IsNotNull(bankResponse.PaymentId);
        }

        [Test]
        public async Task ForwardPaymentFailedResponseTestAsync()
        {
            //Arrange
            var initialPayment = new Payment
            {
                CardNumber = "6783968309687243",
                Cvv = "472",
                ExpiryDate = new DateTime(2022, 10, 12),

                Currency = "USD",
                Amount = 54.56m
            };

            //Act
            var bankResponse = await paymentService.ForwardPayment(initialPayment);

            //Assert
            Assert.AreEqual("FAIL", bankResponse.Status, $"Expected card number to be OK, but it is {bankResponse.Status}");
            Assert.IsNull(bankResponse.PaymentId);
        }

        [Test]
        public void ForwardPaymentFailedAtBankTest()
        {
            //Arrange
            var initialPayment = new Payment
            {
                CardNumber = "6783968309687243",
                Cvv = "472",
                ExpiryDate = new DateTime(2022, 10, 12),

                Currency = "USD",
                Amount = 54.56m,
                MerchantId = 1
            };

            //Act
            var mockBank = new Mock<IBankPayment>();
            mockBank.Setup(x => x.SendPayment(It.IsAny<Payment>()))
                    .ThrowsAsync(new Exception("Bank fail"));

            var failingPaymentService = new PaymentService(mockBank.Object, dataService);

            //Assert
            Assert.ThrowsAsync<Exception>(async () => await failingPaymentService.ForwardPayment(initialPayment));
        }

        [Test]
        public void ForwardPaymentBankSendsEmptyTest()
        {
            //Arrange
            var initialPayment = new Payment
            {
                CardNumber = "6783968309687243",
                Cvv = "472",
                ExpiryDate = new DateTime(2022, 10, 12),

                Currency = "USD",
                Amount = 54.56m,
                MerchantId = 1
            };

            //Act
            var mockBank = new Mock<IBankPayment>();
            mockBank.Setup(x => x.SendPayment(It.IsAny<Payment>())).ReturnsAsync(new BankPaymentResponse { });

            var failingPaymentService = new PaymentService(mockBank.Object, dataService);

            //Assert
            Assert.ThrowsAsync<Exception>(async () => await failingPaymentService.ForwardPayment(initialPayment));
        }

        [Test]
        public void ForwardPaymentBankSendsNullTest()
        {
            //Arrange
            var initialPayment = new Payment
            {
                CardNumber = "6783968309687243",
                Cvv = "472",
                ExpiryDate = new DateTime(2022, 10, 12),

                Currency = "USD",
                Amount = 54.56m,
                MerchantId = 1
            };

            //Act
            var mockBank = new Mock<IBankPayment>();
            mockBank.Setup(x => x.SendPayment(It.IsAny<Payment>())).ReturnsAsync((BankPaymentResponse) null);

            var failingPaymentService = new PaymentService(mockBank.Object, dataService);

            //Assert
            Assert.ThrowsAsync<Exception>(async () => await failingPaymentService.ForwardPayment(initialPayment));
        }

        [Test]
        public async Task ForwardPaymentSavePaymentTestAsync()
        {
            //Arrange
            var initialPayment = new Payment
            {
                CardNumber = "6783968309687243",
                Cvv = "472",
                ExpiryDate = new DateTime(2022, 10, 12),

                Currency = "USD",
                Amount = 54.56m
            };

            //Act
            var bankResponse = await paymentService.ForwardPayment(initialPayment);
            var payment = context.Payments.Find(initialPayment.Id);

            //Assert
            Assert.AreEqual(initialPayment.CardNumber, payment.CardNumber, $"Expected to have card number {initialPayment.CardNumber}, but got {payment.CardNumber}");
        }

        [Test]
        public async Task CorrectPaymentValidationTestAsync()
        {
            //Arrange
            var correctPayment = new PaymentDetails
            {
                CardNumber = "6783968309687243",
                Cvv = "472",
                ExpiryDate = new DateTime(2022, 10, 12),

                Currency = "USD",
                Amount = 54.56m,
                MerchantId = 100
            };
            context.Merchants.Add(new Merchant {Id = 100, Name = "Tesco"});
            context.SaveChanges();

            //Act
            var validated = await paymentValidationService.ValidatePaymentDetails(correctPayment);

            //Assert
            Assert.AreEqual(true, validated, $"Expected to be validated, but it is not.");
        }

        [Test]
        public async Task PaymentValidationNoFieldsTestAsync()
        {
            //Arrange
            var correctPayment = new PaymentDetails
            {
                CardNumber = "6783968309687243",
                Cvv = "472",
                ExpiryDate = new DateTime(2022, 10, 12),

                //Missing currency
                Amount = 54.56m,
                MerchantId = 100
            };
            context.Merchants.Add(new Merchant { Id = 100, Name = "Tesco" });
            context.SaveChanges();

            //Act
            var validated = await paymentValidationService.ValidatePaymentDetails(correctPayment);

            //Assert
            Assert.AreEqual(false, validated, $"Expected not to be validated, but it is.");
        }

        [Test]
        public async Task PaymentIncorrectMerchantValidationTestAsync()
        {
            //Arrange
            var correctPayment = new PaymentDetails
            {
                CardNumber = "6783968309687243",
                Cvv = "472",
                ExpiryDate = new DateTime(2022, 10, 12),

                Currency = "USD",
                Amount = 54.56m,
                MerchantId = 101
            };
            context.Merchants.Add(new Merchant { Id = 100, Name = "Tesco" });
            context.SaveChanges();

            //Act
            var validated = await paymentValidationService.ValidatePaymentDetails(correctPayment);

            //Assert
            Assert.AreEqual(false, validated, $"Expected not to be validated, but it is.");
        }
    }
}
