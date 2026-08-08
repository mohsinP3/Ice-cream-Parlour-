using Ice_Cream_Parlour_Eproject.Data;
using Ice_Cream_Parlour_Eproject.Models;
using Ice_Cream_Parlour_Eproject.Areas.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ice_Cream_Parlour_Eproject.Helpers
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Seed Categories if empty
            if (!await context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Artisan Scoops", Description = "Handcrafted creamy ice cream scoops", IsActive = true, CreatedDate = DateTime.Now.AddMonths(-6) },
                    new Category { Name = "Sundaes & Parfaits", Description = "Rich ice cream sundaes layered with toppings", IsActive = true, CreatedDate = DateTime.Now.AddMonths(-6) },
                    new Category { Name = "Gelato & Sorbet", Description = "Authentic Italian style gelatos and refreshing sorbets", IsActive = true, CreatedDate = DateTime.Now.AddMonths(-6) },
                    new Category { Name = "Ice Cream Cakes", Description = "Celebration ice cream cakes for special events", IsActive = true, CreatedDate = DateTime.Now.AddMonths(-6) },
                    new Category { Name = "Milkshakes & Smoothies", Description = "Thick, creamy milkshakes and fruit blends", IsActive = true, CreatedDate = DateTime.Now.AddMonths(-6) }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            // Seed Recipes if empty
            if (!await context.Recipes.AnyAsync())
            {
                var recipes = new List<Recipe>
                {
                    new Recipe
                    {
                        Name = "Classic Madagascar Vanilla Bean",
                        Category = "Artisan Scoops",
                        Ingredients = "2 cups heavy cream, 1 cup whole milk, 3/4 cup sugar, 1 vanilla bean split, pinch of salt",
                        Procedure = "1. Heat cream, milk, sugar, and scraped vanilla bean pod in sauce pan until warm.\n2. Chill in refrigerator for 4 hours.\n3. Churn in ice cream maker for 25 mins.\n4. Freeze for 2 hours before serving.",
                        ImagePath = "https://images.unsplash.com/photo-1570145820259-b5b80c5c8bd6?q=80&w=600&auto=format&fit=crop",
                        IsFree = true,
                        Price = 0.00m,
                        CreatedDate = DateTime.Now.AddMonths(-5)
                    },
                    new Recipe
                    {
                        Name = "Triple Chocolate Fudge Supreme",
                        Category = "Artisan Scoops",
                        Ingredients = "2 cups heavy cream, 1 cup dark milk, 1/2 cup Dutch cocoa powder, 6oz 70% dark chocolate chunks, 3/4 cup sugar",
                        Procedure = "1. Whisk cocoa powder and sugar into warm milk until dissolved.\n2. Melt dark chocolate into cream mixture.\n3. Chill thoroughly and churn.\n4. Fold in chocolate fudge ribbons.",
                        ImagePath = "https://images.unsplash.com/photo-1563805042-7684c019e1cb?q=80&w=600&auto=format&fit=crop",
                        IsFree = false,
                        Price = 15.00m,
                        CreatedDate = DateTime.Now.AddMonths(-5)
                    },
                    new Recipe
                    {
                        Name = "Fresh Mango & Passionfruit Sorbet",
                        Category = "Gelato & Sorbet",
                        Ingredients = "3 cups fresh mango puree, 1/2 cup passionfruit juice, 3/4 cup simple syrup, 1 tbsp lemon juice",
                        Procedure = "1. Blend fresh ripe mangoes with passionfruit juice and simple syrup.\n2. Strain through fine mesh strainer.\n3. Chill for 2 hours and churn until smooth and icy.\n4. Garnish with mint leaves.",
                        ImagePath = "https://images.unsplash.com/photo-1505394033774-87f57a89ec36?q=80&w=600&auto=format&fit=crop",
                        IsFree = true,
                        Price = 0.00m,
                        CreatedDate = DateTime.Now.AddMonths(-4)
                    },
                    new Recipe
                    {
                        Name = "Sicilian Roasted Pistachio Gelato",
                        Category = "Gelato & Sorbet",
                        Ingredients = "1 cup roasted unsalted pistachios ground into paste, 2 cups milk, 1 cup heavy cream, 3/4 cup sugar, egg yolks",
                        Procedure = "1. Make custard base with milk, sugar, and egg yolks heated to 175°F.\n2. Whisk in rich pistachio paste.\n3. Chill 6 hours, then process in gelato machine for ultra-dense creaminess.",
                        ImagePath = "https://images.unsplash.com/photo-1497034825429-c343d7c6a68f?q=80&w=600&auto=format&fit=crop",
                        IsFree = false,
                        Price = 15.00m,
                        CreatedDate = DateTime.Now.AddMonths(-4)
                    }
                };

                await context.Recipes.AddRangeAsync(recipes);
                await context.SaveChangesAsync();
            }

            // Seed Books if empty
            if (!await context.Books.AnyAsync())
            {
                var books = new List<Book>
                {
                    new Book { Title = "Artisan Ice Cream Making at Home", Author = "Chef Antonio Rossi", Description = "Master 50+ traditional Italian gelatos, sorbets, and frozen desserts.", Price = 24.99m, StockQuantity = 25, ImagePath = "https://images.unsplash.com/photo-1544716278-ca5e3f4abd8c?q=80&w=600&auto=format&fit=crop" },
                    new Book { Title = "The Ultimate Sundae & Toppings", Author = "Sarah Jenkins", Description = "Learn to create hot fudges, caramel sauces, and waffle cones.", Price = 19.99m, StockQuantity = 40, ImagePath = "https://images.unsplash.com/photo-1589829085413-56de8ae18c73?q=80&w=600&auto=format&fit=crop" },
                    new Book { Title = "Dairy-Free & Vegan Frozen Treats", Author = "Maya Lin", Description = "Delicious plant-based sorbets and coconut-cream scoops.", Price = 22.50m, StockQuantity = 15, ImagePath = "https://images.unsplash.com/photo-1512820790803-83ca734da794?q=80&w=600&auto=format&fit=crop" }
                };

                await context.Books.AddRangeAsync(books);
                await context.SaveChangesAsync();
            }

            // Seed Products if empty
            if (!await context.Products.AnyAsync())
            {
                var categories = await context.Categories.ToListAsync();
                var catArtisan = categories.FirstOrDefault(c => c.Name == "Artisan Scoops")?.Id ?? 1;
                var catSundae = categories.FirstOrDefault(c => c.Name == "Sundaes & Parfaits")?.Id ?? 2;
                var catGelato = categories.FirstOrDefault(c => c.Name == "Gelato & Sorbet")?.Id ?? 3;
                var catCake = categories.FirstOrDefault(c => c.Name == "Ice Cream Cakes")?.Id ?? 4;
                var catShake = categories.FirstOrDefault(c => c.Name == "Milkshakes & Smoothies")?.Id ?? 5;

                var products = new List<Product>
                {
                    new Product { Name = "Madagascar Vanilla Bean", Description = "Classic rich vanilla made with real Madagascar vanilla pods.", CategoryId = catArtisan, Price = 8.50m, DiscountPercent = 10m, StockQuantity = 45, LowStockThreshold = 10, Barcode = "123456789012", ProductCode = "PROD-VAN-01", ImagePath = "https://images.unsplash.com/photo-1570145820259-b5b80c5c8bd6?q=80&w=600&auto=format&fit=crop", Status = ProductStatus.Active, CreatedAt = DateTime.Now.AddMonths(-3) },
                    new Product { Name = "Belgian Dark Chocolate Fudge", Description = "Deep 70% dark Belgian chocolate scoop swirled with decadent hot fudge.", CategoryId = catArtisan, Price = 9.99m, DiscountPercent = 15m, StockQuantity = 30, LowStockThreshold = 10, Barcode = "123456789013", ProductCode = "PROD-CHO-02", ImagePath = "https://images.unsplash.com/photo-1563805042-7684c019e1cb?q=80&w=600&auto=format&fit=crop", Status = ProductStatus.Active, CreatedAt = DateTime.Now.AddMonths(-3) },
                    new Product { Name = "Fresh Strawberry Shortcake", Description = "Made with organic ripe strawberries and graham cracker biscuit crumbles.", CategoryId = catGelato, Price = 10.50m, DiscountPercent = 0m, StockQuantity = 22, LowStockThreshold = 5, Barcode = "123456789014", ProductCode = "PROD-STR-03", ImagePath = "https://images.unsplash.com/photo-1497034825429-c343d7c6a68f?q=80&w=600&auto=format&fit=crop", Status = ProductStatus.Active, CreatedAt = DateTime.Now.AddMonths(-3) },
                    new Product { Name = "Mango Passion Fruit Sorbet", Description = "100% dairy-free tropical mango and tangy passionfruit sorbet.", CategoryId = catGelato, Price = 7.99m, DiscountPercent = 5m, StockQuantity = 50, LowStockThreshold = 10, Barcode = "123456789015", ProductCode = "PROD-MAN-04", ImagePath = "https://images.unsplash.com/photo-1505394033774-87f57a89ec36?q=80&w=600&auto=format&fit=crop", Status = ProductStatus.Active, CreatedAt = DateTime.Now.AddMonths(-3) },
                    new Product { Name = "Sicilian Pistachio Crunch", Description = "Roasted Bronte pistachios blended into silky smooth Italian gelato.", CategoryId = catGelato, Price = 11.25m, DiscountPercent = 0m, StockQuantity = 8, LowStockThreshold = 10, Barcode = "123456789016", ProductCode = "PROD-PIS-05", ImagePath = "https://images.unsplash.com/photo-1501443715940-a536eae44c1f?q=80&w=600&auto=format&fit=crop", Status = ProductStatus.Active, CreatedAt = DateTime.Now.AddMonths(-3) },
                    new Product { Name = "Salted Caramel Butterscotch", Description = "Caramelized brown sugar with sea salt chunks and butterscotch crunch.", CategoryId = catSundae, Price = 12.00m, DiscountPercent = 20m, StockQuantity = 3, LowStockThreshold = 5, Barcode = "123456789017", ProductCode = "PROD-CAR-06", ImagePath = "https://images.unsplash.com/photo-1580915411954-282cb1b0d780?q=80&w=600&auto=format&fit=crop", Status = ProductStatus.Active, CreatedAt = DateTime.Now.AddMonths(-3) }
                };

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }

            // Seed Customers if empty
            if (!await context.Customers.AnyAsync())
            {
                var customers = new List<Customer>
                {
                    new Customer { FullName = "John Anderson", Email = "user@icream.com", Phone = "+1 (555) 234-5678", Address = "456 Sweet Ave, New York, NY 10001", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-2), LastActivityDate = DateTime.Now },
                    new Customer { FullName = "Maria Garcia", Email = "maria@gmail.com", Phone = "+1 (555) 987-6543", Address = "789 Cream Way, Brooklyn, NY 11201", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-2), LastActivityDate = DateTime.Now },
                    new Customer { FullName = "Robert Wilson", Email = "robert.w@gmail.com", Phone = "+1 (555) 345-6789", Address = "101 Ice Street, Manhattan, NY 10002", IsActive = true, CreatedAt = DateTime.Now.AddMonths(-2), LastActivityDate = DateTime.Now }
                };

                await context.Customers.AddRangeAsync(customers);
                await context.SaveChangesAsync();
            }

            // Seed Orders and OrderItems if empty
            if (!await context.Orders.AnyAsync())
            {
                var customers = await context.Customers.ToListAsync();
                var customer1 = customers[0];
                var customer2 = customers[1];

                var products = await context.Products.ToListAsync();
                var books = await context.Books.ToListAsync();

                var order1 = new Order
                {
                    OrderNumber = "ORD-" + DateTime.Now.AddDays(-5).ToString("yyyyMMdd") + "-1042",
                    OrderDate = DateTime.Now.AddDays(-5),
                    CustomerId = customer1.Id,
                    CustomerName = customer1.FullName,
                    CustomerEmail = customer1.Email,
                    CustomerPhone = customer1.Phone,
                    DeliveryAddress = customer1.Address,
                    TotalAmount = 28.49m,
                    OrderStatus = "Delivered",
                    PaymentStatus = "Paid",
                    PaymentMethod = "Credit Card",
                    Notes = "Please drop off at reception desk",
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem { ProductId = products[0].Id, ProductName = products[0].Name, Quantity = 2, UnitPrice = products[0].Price },
                        new OrderItem { ProductId = products[1].Id, ProductName = products[1].Name, Quantity = 1, UnitPrice = products[1].Price }
                    }
                };

                var order2 = new Order
                {
                    OrderNumber = "ORD-" + DateTime.Now.AddDays(-2).ToString("yyyyMMdd") + "-2198",
                    OrderDate = DateTime.Now.AddDays(-2),
                    CustomerId = customer2.Id,
                    CustomerName = customer2.FullName,
                    CustomerEmail = customer2.Email,
                    CustomerPhone = customer2.Phone,
                    DeliveryAddress = customer2.Address,
                    TotalAmount = 45.00m,
                    OrderStatus = "Processing",
                    PaymentStatus = "Paid",
                    PaymentMethod = "PayPal",
                    Notes = "Include birthday candle package",
                    BookId = books[0].Id,
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem { ProductId = products[2].Id, ProductName = products[2].Name, Quantity = 1, UnitPrice = products[2].Price }
                    }
                };

                await context.Orders.AddRangeAsync(new[] { order1, order2 });
                await context.SaveChangesAsync();
            }

            // Seed Feedbacks if empty
            if (!await context.Feedbacks.AnyAsync())
            {
                var feedbacks = new List<Feedback>
                {
                    new Feedback { UserName = "John Anderson", Email = "user@icream.com", Message = "The Madagascar Vanilla Bean and Pistachio gelatos are absolutely mind blowing! Highly recommend joining membership.", Rating = 5, SubmittedDate = DateTime.Now.AddDays(-3), IsRegistered = true, IsRead = true },
                    new Feedback { UserName = "Maria Garcia", Email = "maria@gmail.com", Message = "Loved the fast delivery and packaging. The ice cream arrived completely frozen even in heat!", Rating = 5, SubmittedDate = DateTime.Now.AddDays(-1), IsRegistered = true, IsRead = false }
                };

                await context.Feedbacks.AddRangeAsync(feedbacks);
                await context.SaveChangesAsync();
            }
        }
    }
}