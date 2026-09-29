using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<City> Cities =>
        Set<City>();

    public DbSet<Restaurant> Restaurants =>
        Set<Restaurant>();

    public DbSet<MenuItem> MenuItems =>
        Set<MenuItem>();

    public DbSet<Order> Orders =>
        Set<Order>();

    public DbSet<OrderItem> OrderItems =>
        Set<OrderItem>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ============================================================
        // CITY
        // ============================================================

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(x => x.Name)
                .IsUnique();
        });

        // ============================================================
        // RESTAURANT
        // ============================================================

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.Category)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(x => x.ImageUrl)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(x => x.Rating)
                .HasPrecision(3, 2);

            entity.HasOne(x => x.City)
                .WithMany(x => x.Restaurants)
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.CityId);

            entity.HasIndex(x => x.Name);
        });

        // ============================================================
        // MENU ITEM
        // ============================================================

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(x => x.Category)
                .IsRequired()
                .HasMaxLength(80);

            entity.Property(x => x.Price)
                .HasPrecision(10, 2);

            entity.Property(x => x.ImageUrl)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(x => x.IsAvailable)
                .HasDefaultValue(true);

            entity.HasOne(x => x.Restaurant)
                .WithMany(x => x.MenuItems)
                .HasForeignKey(x => x.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => x.RestaurantId);

            entity.HasIndex(x => new
            {
                x.RestaurantId,
                x.IsAvailable
            });
        });

        // ============================================================
        // ORDER
        // ============================================================

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.RestaurantName)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.RestaurantImageUrl)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(x => x.CustomerName)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.CustomerPhone)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(x => x.CustomerEmail)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.CustomerAddress)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(x => x.Subtotal)
                .HasPrecision(10, 2);

            entity.Property(x => x.DeliveryFee)
                .HasPrecision(10, 2);

            entity.Property(x => x.Total)
                .HasPrecision(10, 2);

            entity.Property(x => x.CreatedAt)
                .IsRequired();

            entity.HasIndex(x => x.RestaurantId);

            entity.HasIndex(x => x.CreatedAt);

            entity.HasMany(x => x.Items)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ============================================================
        // ORDER ITEM
        // ============================================================

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.ImageUrl)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(x => x.UnitPrice)
                .HasPrecision(10, 2);

            entity.Property(x => x.LineTotal)
                .HasPrecision(10, 2);

            entity.Property(x => x.Quantity)
                .IsRequired();

            entity.HasOne(x => x.Order)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.MenuItem)
                .WithMany()
                .HasForeignKey(x => x.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.OrderId);

            entity.HasIndex(x => x.MenuItemId);
        });
    }
}