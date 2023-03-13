using Infrastructure.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase
{
    public class EfCoreContext : DbContext
    {
        //public EfCoreContext()
        //{
        //    Database.EnsureDeleted();
        //    Database.EnsureCreated();
        //}

        public EfCoreContext(DbContextOptions<EfCoreContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        //public static EfCoreContext Context { get; } = new();

        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> Items { get; set; }
        public DbSet<Provider> Providers { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);

        //    optionsBuilder.UseSqlServer(Constants.CreatingString);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                        .Property(model => model.Id)
                        .ValueGeneratedOnAdd();

            modelBuilder.Entity<OrderItem>()
                        .Property(model => model.Id)
                        .ValueGeneratedOnAdd();

            modelBuilder.Entity<Provider>()
                        .Property(model => model.Id)
                        .ValueGeneratedOnAdd();

            modelBuilder.Entity<Order>()
                        .HasIndex(order => new
                        {
                            order.Number,
                            order.ProviderId
                        }, "OrderNumberProviderId_Index")
                        .IsUnique();

            modelBuilder.Entity<OrderItem>()
                        .Property(item => item.Quantity)
                        .HasPrecision(18, 3);

            modelBuilder.Entity<Provider>()
                        .HasMany(provider => provider.Orders)
                        .WithOne(order => order.Provider)
                        .HasForeignKey(order => order.ProviderId);

            modelBuilder.Entity<Order>()
                        .HasMany(order => order.Items)
                        .WithOne(item => item.Order)
                        .HasForeignKey(order => order.OrderId);
        }
    }
}