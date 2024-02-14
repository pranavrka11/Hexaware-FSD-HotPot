using HotPot.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPot.Context
{
    public class HotpotContext : DbContext
    {
        public HotpotContext(DbContextOptions options) : base(options)
        {
            
        }


        public DbSet<City>? Cities { get; set; }
        public DbSet<DeliveryPartner>? DeliveryPartners { get; set; }
        public DbSet<Menu>? Menus { get; set; }
        public DbSet<NutritionalInfo>? NutritionalInfos { get; set;}
        public DbSet<Order>? Orders { get; set; }
        public DbSet<OrderItem>? OrderItems { get; set;}
        public DbSet<Payment>? Payments { get; set; }
        public DbSet<Restaurant>? Restaurants { get; set; } 
        public DbSet<RestaurantSpeciality>? RestaurantSpecialities { get; set; }
        public DbSet<State>? States { get; set; }
        public DbSet<Customer>? Customers { get; set; }
        public DbSet<CustomerAddress>? CustomerAddresses { get; set; }
        public DbSet<CustomerReview>? CustomerReviews { get; set; }
        public DbSet<RestaurantOwner>? RestaurantOwners { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Cart>? Carts { get; set; }

        /// <summary>
        /// Configures the model for the database context, specifically handling composite keys for OrderItem entities.
        /// </summary>
        /// <param name="modelBuilder">The builder for configuring the model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure composite key for OrderItem entity
            modelBuilder.Entity<OrderItem>()
                .HasKey(o => new { o.OrderId, o.MenuId });

            // Configure relationships for OrderItem entity
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Menu)
                .WithMany(m => m.OrderItems)
                .HasForeignKey(oi => oi.MenuId)
                .OnDelete(DeleteBehavior.NoAction);

            //configure relationship for the 
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Restaurant)
                .WithMany()
                .HasForeignKey(c => c.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
