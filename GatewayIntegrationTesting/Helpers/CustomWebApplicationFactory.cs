using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore;
using APIGateway;
using APIGateway.Models.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GatewayIntegrationTesting.Helpers
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup> 
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Add a database context (ApplicationDbContext) using an in-memory 
                // database for testing.
                services.AddDbContext<GatewayContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                    options.UseInternalServiceProvider(serviceProvider);
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var ctx = scopedServices.GetRequiredService<GatewayContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory>>();

                    // Ensure the database is created.
                    ctx.Database.EnsureCreated();

                    try
                    {
                        ctx.Merchants.Add(new Merchant
                        {
                            Name = "Apple",
                            Id = 1,
                            Username = "AppleOffice",
                            Password = "N5HtcH/3iA4m2gts5flQ6PWXohHMjWxlhCSSWtIqZv4=",
                            Salt =
                                "APPLESaltIsAuniqueSequenceOfCharactersUsedPerUserInOrderToPreventAgainstRainbowAttacksAndShouldBeRandomlyGenerated"
                        });
                        ctx.Merchants.Add(new Merchant
                        {
                            Name = "Sony",
                            Id = 2,
                            Username = "SonyOffice",
                            Password = "Azd9hg0jL60Lbvxne7pLRjq/KjS3tU2lpAKLZqH/uis=",
                            Salt =
                                "SONYSaltIsAuniqueSequenceOfCharactersUsedPerUserInOrderToPreventAgainstRainbowAttacksAndShouldBeRandomlyGenerated"
                        });
                        ctx.Merchants.Add(new Merchant
                        {
                            Name = "Primark",
                            Id = 3,
                            Username = "PrimarkRetail",
                            Password = "Io4b5E5zzIKzYF1LZFCP7iWSd5e6L6SgtpiF7ERnC5I=",
                            Salt =
                                "PRIMARKSaltIsAuniqueSequenceOfCharactersUsedPerUserInOrderToPreventAgainstRainbowAttacksAndShouldBeRandomlyGenerated"
                        });
                        ctx.Payments.Add(new Payment
                        {
                            Id = 100,
                            Amount = 44.50m,
                            BankUniqueId = "id100am44.50",
                            CardNumber = "532675437436532",
                            Currency = "GBP",
                            Cvv = "345",
                            ExpiryDate = new DateTime(2021, 2, 14),
                            MerchantId = 1,
                            Status = Constants.BankResponses.Success
                        });
                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"An error occurred seeding the " +
                                            "database with test messages. Error: {ex.Message}");
                    }
                }
            });
        }
    }

}
