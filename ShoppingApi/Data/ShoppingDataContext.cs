using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Data
{
    public class ShoppingDataContext : DbContext
    {
        public ShoppingDataContext(DbContextOptions<ShoppingDataContext> options) : base(options)
        {

        }
        public DbSet<ShoppingItem> ShoppingItems { get; set; }
    }
}
