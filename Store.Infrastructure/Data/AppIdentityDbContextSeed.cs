using Microsoft.AspNetCore.Identity;
using Store.Core.Entities;
using System.Text.Json;

namespace Store.Infrastructure.Data
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(StoreContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            // Seed Roles
            if (!roleManager.Roles.Any())
            {
                var roles = new List<IdentityRole<int>>
                {
                    new IdentityRole<int> { Name = "Admin" },
                    new IdentityRole<int> { Name = "Customer" },
                    new IdentityRole<int> { Name = "SuperAdmin" }
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }

            // Seed Users
            if (!userManager.Users.Any())
            {
                // 1 SuperAdmin
                var superAdmin = new ApplicationUser
                {
                    DisplayName = "Super Admin",
                    Email = "superadmin@store.com",
                    UserName = "superadmin@store.com"
                };
                await userManager.CreateAsync(superAdmin, "Pa$$w0rd");
                await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");

                // 2 Admins
                var admin1 = new ApplicationUser { DisplayName = "Admin One", Email = "admin1@store.com", UserName = "admin1@store.com" };
                var admin2 = new ApplicationUser { DisplayName = "Admin Two", Email = "admin2@store.com", UserName = "admin2@store.com" };
                
                await userManager.CreateAsync(admin1, "Pa$$w0rd");
                await userManager.AddToRoleAsync(admin1, "Admin");
                
                await userManager.CreateAsync(admin2, "Pa$$w0rd");
                await userManager.AddToRoleAsync(admin2, "Admin");

                // 2 Customers
                var customer1 = new ApplicationUser { DisplayName = "John Doe", Email = "john@customer.com", UserName = "john@customer.com" };
                var customer2 = new ApplicationUser { DisplayName = "Jane Smith", Email = "jane@customer.com", UserName = "jane@customer.com" };
                
                await userManager.CreateAsync(customer1, "Pa$$w0rd");
                await userManager.AddToRoleAsync(customer1, "Customer");
                
                await userManager.CreateAsync(customer2, "Pa$$w0rd");
                await userManager.AddToRoleAsync(customer2, "Customer");
            }

            // Seed Addresses (independent of user seeding)
            if (!context.Addresses.Any())
            {
                var superAdmin = await userManager.FindByEmailAsync("superadmin@store.com");
                var admin1 = await userManager.FindByEmailAsync("admin1@store.com");
                var admin2 = await userManager.FindByEmailAsync("admin2@store.com");
                var customer1 = await userManager.FindByEmailAsync("john@customer.com");
                var customer2 = await userManager.FindByEmailAsync("jane@customer.com");

                if (superAdmin != null && admin1 != null && admin2 != null && customer1 != null && customer2 != null)
                {
                    var addresses = new List<Address>
                    {
                        new Address
                        {
                            FirstName = "Super",
                            LastName = "Admin",
                            Street = "123 Admin Street",
                            City = "New York",
                            State = "NY",
                            ZipCode = "10001",
                            ApplicationUserId = superAdmin.Id
                        },
                        new Address
                        {
                            FirstName = "Admin",
                            LastName = "One",
                            Street = "456 Manager Ave",
                            City = "Los Angeles",
                            State = "CA",
                            ZipCode = "90001",
                            ApplicationUserId = admin1.Id
                        },
                        new Address
                        {
                            FirstName = "Admin",
                            LastName = "Two",
                            Street = "789 Executive Blvd",
                            City = "Chicago",
                            State = "IL",
                            ZipCode = "60601",
                            ApplicationUserId = admin2.Id
                        },
                        new Address
                        {
                            FirstName = "John",
                            LastName = "Doe",
                            Street = "321 Customer Lane",
                            City = "Houston",
                            State = "TX",
                            ZipCode = "77001",
                            ApplicationUserId = customer1.Id
                        },
                        new Address
                        {
                            FirstName = "Jane",
                            LastName = "Smith",
                            Street = "654 Shopper Road",
                            City = "Phoenix",
                            State = "AZ",
                            ZipCode = "85001",
                            ApplicationUserId = customer2.Id
                        }
                    };

                    context.Addresses.AddRange(addresses);
                    await context.SaveChangesAsync();
                }
            }

            // Seed Brands
            if (!context.ProductBrands.Any())
            {
                var brands = new List<ProductBrand>
                {
                    new ProductBrand { Name = "Apple" },
                    new ProductBrand { Name = "Samsung" },
                    new ProductBrand { Name = "Sony" },
                    new ProductBrand { Name = "Microsoft" },
                    new ProductBrand { Name = "Dell" }
                };
                context.ProductBrands.AddRange(brands);
                await context.SaveChangesAsync();
            }

            // Seed Types
            if (!context.ProductTypes.Any())
            {
                var types = new List<ProductType>
                {
                    new ProductType { Name = "Laptop" },
                    new ProductType { Name = "Smartphone" },
                    new ProductType { Name = "Headphones" },
                    new ProductType { Name = "Console" },
                    new ProductType { Name = "Monitor" }
                };
                context.ProductTypes.AddRange(types);
                await context.SaveChangesAsync();
            }

            // Seed Categories
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Electronics", Description = "Devices and gadgets" },
                    new Category { Name = "Gaming", Description = "Everything for gamers" },
                    new Category { Name = "Computer", Description = "Computing hardware" },
                    new Category { Name = "Accessories", Description = "Tech accessories" },
                    new Category { Name = "Audio", Description = "Sound equipment" }
                };
                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // Seed Products
            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product { Name = "MacBook Pro", Description = "Powerful laptop", Price = 2000, PictureUrl = "images/macbook.png", ProductBrandId = 1, ProductTypeId = 1, CategoryId = 3 },
                    new Product { Name = "iPhone 15", Description = "Latest smartphone", Price = 1000, PictureUrl = "images/iphone.png", ProductBrandId = 1, ProductTypeId = 2, CategoryId = 1 },
                    new Product { Name = "Galaxy S23", Description = "Android flagship", Price = 900, PictureUrl = "images/galaxy.png", ProductBrandId = 2, ProductTypeId = 2, CategoryId = 1 },
                    new Product { Name = "WH-1000XM5", Description = "Noise cancelling headphones", Price = 350, PictureUrl = "images/sonyh.png", ProductBrandId = SonyBrandId(context), ProductTypeId = 3, CategoryId = 5 },
                    new Product { Name = "Xbox Series X", Description = "Power your dreams", Price = 500, PictureUrl = "images/xbox.png", ProductBrandId = 4, ProductTypeId = 4, CategoryId = 2 }
                };
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }
        }

        private static int SonyBrandId(StoreContext context) => context.ProductBrands.First(b => b.Name == "Sony").Id;
    }
}
