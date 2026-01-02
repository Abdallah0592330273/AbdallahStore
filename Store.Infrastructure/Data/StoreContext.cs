using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;

namespace Store.Infrastructure.Data
{
    public class StoreContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<OrderItem>().Property(i => i.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Order>().Property(o => o.Subtotal).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<DeliveryMethod>().Property(d => d.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Discount>().Property(d => d.Amount).HasColumnType("decimal(18,2)");

            // Configure Order ownership of ShipToAddress and ProductItemOrdered
            modelBuilder.Entity<Order>().OwnsOne(o => o.ShipToAddress, a => {
                a.WithOwner();
            });

            modelBuilder.Entity<OrderItem>().OwnsOne(i => i.ItemOrdered, io => {
                io.WithOwner();
            });

            // Configure 1-to-1 relationship between ApplicationUser and Address
            modelBuilder.Entity<Address>()
                .HasOne(a => a.ApplicationUser)
                .WithOne(u => u.Address)
                .HasForeignKey<Address>(a => a.ApplicationUserId);

            // Configure 1-to-1 relationship between ApplicationUser and Wishlist
            modelBuilder.Entity<Wishlist>()
                .HasOne(a => a.ApplicationUser)
                .WithOne(u => u.Wishlist)
                .HasForeignKey<Wishlist>(a => a.ApplicationUserId);
        }
    }
}
