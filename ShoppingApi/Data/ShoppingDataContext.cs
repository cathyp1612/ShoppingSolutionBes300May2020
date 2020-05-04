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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(LoggerFactory);
        }
    }
}
