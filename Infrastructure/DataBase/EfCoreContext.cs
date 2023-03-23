using Infrastructure.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase
{
    public class EfCoreContext : DbContext
    {
        public EfCoreContext(DbContextOptions<EfCoreContext> options) : base(options)
        {
            if(Database.EnsureCreated())
            {
                Database.Migrate();
            }
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Provider> Providers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                        .ToTable(nameof(Order))
                        .Property(model => model.Id)
                        .ValueGeneratedOnAdd();

            modelBuilder.Entity<Item>()
                        .ToTable("OrderItem")
                        .Property(model => model.Id)
                        .ValueGeneratedOnAdd();

            modelBuilder.Entity<Provider>()
                        .ToTable(nameof(Provider))
                        .Property(model => model.Id)
                        .ValueGeneratedOnAdd();

            modelBuilder.Entity<Order>()
                        .HasIndex(order => new
                        {
                            order.Number,
                            order.ProviderId
                        }, "OrderNumberProviderId_Index")
                        .IsUnique();

            modelBuilder.Entity<Item>().Property(item => item.Quantity).HasPrecision(18, 3);

            modelBuilder.Entity<Provider>()
                        .HasMany(provider => provider.Orders)
                        .WithOne(order => order.Provider)
                        .HasForeignKey(order => order.ProviderId);

            //modelBuilder.Entity<Order>()
            //            .HasOne(order => order.Provider)
            //            .WithMany(provider => provider.Orders)
            //            .HasForeignKey(order => order.ProviderId);

            modelBuilder.Entity<Order>()
                        .HasMany(order => order.Items)
                        .WithOne(item => item.Order)
                        .HasForeignKey(order => order.OrderId);
        }
    }
}