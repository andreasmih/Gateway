using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Models.DB
{
    public class GatewayContext : DbContext
    {
        public GatewayContext(DbContextOptions<GatewayContext> options) : base(options)
        { }

        public GatewayContext(): base() { }

        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasOne(e => e.Merchant)
                    .WithMany(e => e.Payments)
                    .HasForeignKey(e => e.MerchantId);
            });

        }
    }
}
