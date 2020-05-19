using System;
using System.Collections.Generic;
using System.Text;
using GatewayIntegrationTesting.Helpers;
using NUnit.Framework;

namespace GatewayIntegrationTesting
{
    public class IntegrationTestBase
    {
        protected CustomWebApplicationFactory _clientFactory;

        [OneTimeSetUp]
        public void GivenARequestToTheController()
        {
            _clientFactory = new CustomWebApplicationFactory();
        }
    }
}
