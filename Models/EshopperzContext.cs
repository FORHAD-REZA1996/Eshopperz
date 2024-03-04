using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Eshopperz.Models;

namespace Eshopperz.Models
{
    public class EshopperzContext : DbContext
    {
        public EshopperzContext(DbContextOptions<EshopperzContext> options) : base(options)
        {
        }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>()
                .HasKey(OrderItem => new { OrderItem.OrderId, OrderItem.ProductId, OrderItem.ProductOrderDate  });
            
            // Other entity configurations...

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Eshopperz.Models.Admin> Admin { get; set; } = default!;


    }
}