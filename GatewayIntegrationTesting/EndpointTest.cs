using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using APIGateway;
using APIGateway.Helpers;
using APIGateway.Models;
using GatewayIntegrationTesting.Helpers;
using NUnit.Framework;

namespace GatewayIntegrationTesting
{
    public class EndpointTest : IntegrationTestBase
    {
        [Test]
        public async Task TestAddPaymentPassAsync()
        {
            //Arrange
            var cont = new
            {
                card_number = "6436643297557554",
                expiry_date = new DateTime(2020, 10, 23, 0, 0, 0),
                cvv = "425",
                amount = 46.54m,
                currency = "GBP",
                merchant_id = 1
            };

            //Act
            using (var client = _clientFactory.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "SonyOffice:password");
                var response = await client.PostAsJsonAsync("payment", cont);

                var respString = await response.Content.ReadAsStringAsync();
                var respObj = JsonSerializer.Deserialize<PaymentMerchantResponse>(respString, 
                    new JsonSerializerOptions{ PropertyNamingPolicy = new SnakeCaseNamingPolicy()});

                //Assert
                Assert.IsNotNull(respObj.Id);
                Assert.AreEqual($"id{respObj.Id}am{cont.amount}", respObj.BankReference);
                Assert.AreEqual(Constants.BankResponses.Success, respObj.Status);
            }
        }

        [Test]
        public async Task TestAddPaymentFailAsync()
        {
            //Arrange
            var cont = new
            {
                card_number = "6436643297557554",
                expiry_date = new DateTime(2020, 10, 23, 0, 0, 0),
                cvv = "425",
                amount = 56.54m,
                currency = "GBP",
                merchant_id = 1
            };

            //Act
            using (var client = _clientFactory.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "SonyOffice:password");
                var response = await client.PostAsJsonAsync("payment", cont);

                var respString = await response.Content.ReadAsStringAsync();
                var respObj = JsonSerializer.Deserialize<PaymentMerchantResponse>(respString,
                    new JsonSerializerOptions { PropertyNamingPolicy = new SnakeCaseNamingPolicy() });

                //Assert
                Assert.IsNotNull(respObj.Id); 
                Assert.IsNull(respObj.BankReference);
                Assert.AreEqual(Constants.BankResponses.Failed, respObj.Status);
            }
        }

        [Test]
        public async Task TestGetPaymentPassAsync()
        {
            //Arrange
            

            //Act
            using (var client = _clientFactory.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "SonyOffice:password");
                var response = await client.GetAsync("payment?id=100");
                var respString = await response.Content.ReadAsStringAsync();
                var respObj = JsonSerializer.Deserialize<PaymentEnquiryResponse>(respString,
                    new JsonSerializerOptions { PropertyNamingPolicy = new SnakeCaseNamingPolicy() });

                //Assert
                Assert.AreEqual(Constants.BankResponses.Success,respObj.Status);
                Assert.AreEqual(44.50m, respObj.Amount);
                Assert.AreEqual("GBP", respObj.Currency);
                Assert.AreEqual("345", respObj.Cvv);
                Assert.AreEqual(new DateTime(2021, 2, 14), respObj.ExpiryDate);
                Assert.IsTrue(respObj.MaskedCardNumber.Contains("********"));
            }
        }

        [Test]
        public async Task TestGetPaymentFailAsync()
        {
            //Arrange


            //Act
            using (var client = _clientFactory.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "SonyOffice:password");
                var response = await client.GetAsync("payment?id=105");
                var respString = await response.Content.ReadAsStringAsync();
                
                //Assert
                Assert.AreEqual("Id 105 does not exist", respString);
            }
        }
    }
}
