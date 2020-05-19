using System;
using System.Collections.Generic;
using System.Text;
using APIGateway.Models.DB;
using APIGateway.Models.Mappers;
using APIGateway.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace GatewayTests
{
    public class TestBase
    {
        protected IMapper mapper => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new GatewayMapper());
        }).CreateMapper();

        protected GatewayContext context;
        protected DataService dataService => new DataService(context, new Mock<ILogger<DataService>>().Object);
        protected BankPaymentService bankPaymentService => new BankPaymentService(new Mock<ILogger<BankPaymentService>>().Object);
        protected PaymentService paymentService => new PaymentService(bankPaymentService, dataService);
        protected PaymentValidationService paymentValidationService => new PaymentValidationService(dataService);

        [SetUp]
        protected void Setup()
        {
            var dbOptions = new DbContextOptionsBuilder<GatewayContext>()
                .UseInMemoryDatabase(databaseName: "Testing")
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            
            this.context = new GatewayContext(dbOptions);
        }

        [TearDown]
        protected void TearDown()
        {
            context.Database.EnsureDeleted();
        }
    }
}
