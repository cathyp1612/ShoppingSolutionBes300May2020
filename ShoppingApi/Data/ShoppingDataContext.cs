using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Data
{
    public class ShoppingDataContext : DbContext
    {
        private readonly ILoggerFactory LoggerFactory;

        public ShoppingDataContext
            (DbContextOptions<ShoppingDataContext> options,
            ILoggerFactory loggerFactory) : base(options)
        {
            LoggerFactory = loggerFactory;
        }
        public DbSet<ShoppingItem> ShoppingItems { get; set; }
        public DbSet<OrderForCurbside> CurbsideOrders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(LoggerFactory);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingItem>().HasData(
                new ShoppingItem { Id = 1, Description = "Beer", Purchased = false },
                new ShoppingItem { Id = 2, Description = "Toilet Paper", Purchased = true, PurchasedFrom = "Acme" }
                );
        }
    }
}
