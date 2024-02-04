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
        public DbSet<OrderItem>? OrdersItems { get; set;}
        public DbSet<Payment>? Payments { get; set; }
        public DbSet<Restaurant>? Restaurants { get; set; } 
        public DbSet<RestaurantSpeciality>? RestaurantsSpecialities { get; set; }
        public DbSet<State>? States { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<UserAddress>? UserAddresses { get; set; }
        public DbSet<UserReviews>? UserReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>()
                .HasKey(o => new { o.OrderId, o.MenuId });

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
        }
    }
}
