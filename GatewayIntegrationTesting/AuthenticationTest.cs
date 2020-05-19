using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NUnit.Framework;

namespace GatewayIntegrationTesting
{
    public class AuthenticationTest : IntegrationTestBase
    {

        [Test]
        public async Task TestAuthenticationFailAsync()
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
                var response = await client.PostAsJsonAsync("payment", cont);

                //Assert
                Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }

        [Test]
        public async Task TestAuthenticationPassWrongPasswordAsync()
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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic","SonyOffice:wrongpassword");
                var response = await client.PostAsJsonAsync("payment", cont);

                //Assert
                Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }

        [Test]
        public async Task TestAuthenticationPassAsync()
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

                //Assert
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}