using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ice_Cream_Parlour_Eproject.Data;
using Ice_Cream_Parlour_Eproject.Models;
using Ice_Cream_Parlour_Eproject.Areas.Models.ViewModels;

namespace Ice_Cream_Parlour_Eproject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var todaySales = (await _context.Orders
                .Where(o => o.OrderDate.Date == DateTime.Today.Date)
                .Select(o => o.TotalAmount)
                .ToListAsync()).Sum();

            var monthlySales = (await _context.Orders
                .Where(o => o.OrderDate.Month == DateTime.Now.Month && o.OrderDate.Year == DateTime.Now.Year)
                .Select(o => o.TotalAmount)
                .ToListAsync()).Sum();

            var lowStockProducts = await _context.Products
                .Where(p => p.StockQuantity <= p.LowStockThreshold)
                .Select(p => new LowStockProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    StockQuantity = p.StockQuantity,
                    LowStockThreshold = p.LowStockThreshold,
                    StockPercent = p.LowStockThreshold > 0 ? (int)((double)p.StockQuantity / p.LowStockThreshold * 100) : 0
                })
                .Take(5)
                .ToListAsync();

            var allOrderItems = await _context.Orders
                .SelectMany(o => o.OrderItems)
                .Select(oi => new { oi.ProductId, oi.ProductName, oi.Quantity, oi.UnitPrice })
                .ToListAsync();

            var topSellingProducts = allOrderItems
                .GroupBy(oi => new { oi.ProductId, oi.ProductName })
                .Select(g => new TopSellingProductDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    TotalSold = g.Sum(oi => oi.Quantity),
                    Revenue = g.Sum(oi => oi.Quantity * oi.UnitPrice)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(5)
                .ToList();

            var model = new DashboardViewModel
            {
                TotalOrders = await _context.Orders.CountAsync(),
                TotalCustomers = await _context.Customers.CountAsync(),
                TotalProducts = await _context.Products.CountAsync(),
                TotalCategories = await _context.Categories.CountAsync(),
                TodaySales = todaySales,
                MonthlySales = monthlySales,
                RevenueChartLabels = GetLast7DaysLabels(),
                RevenueChartData = await GetLast7DaysData(),
                LowStockProducts = lowStockProducts,
                TopSellingProducts = topSellingProducts
            };

            // Calculate Progress Percent
            if (model.TopSellingProducts.Any())
            {
                var maxSold = model.TopSellingProducts.Max(x => x.TotalSold);
                foreach (var item in model.TopSellingProducts)
                {
                    item.ProgressPercent = maxSold > 0 ? (int)((double)item.TotalSold / maxSold * 100) : 0;
                } 
            }

            return View(model);
        }

        private List<string> GetLast7DaysLabels()
        {
            return Enumerable.Range(0, 7)
                .Select(i => DateTime.Today.AddDays(-i).ToString("ddd"))
                .Reverse()
                .ToList();
        }

        private async Task<List<decimal>> GetLast7DaysData()
        {
            var data = new List<decimal>();
            for (int i = 6; i >= 0; i--)
            {
                var date = DateTime.Today.AddDays(-i);
                var total = (await _context.Orders
                    .Where(o => o.OrderDate.Date == date.Date)
                    .Select(o => o.TotalAmount)
                    .ToListAsync()).Sum();
                data.Add(total);
            }
            return data;
        }

        // ===== PRODUCT MANAGEMENT (Recipes) =====
        public async Task<IActionResult> Products()
        {
            var products = await _context.Recipes.ToListAsync();
            return View(products);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Recipe recipe, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                // Image Upload
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var folder = "images/recipes/";
                    var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageFile.FileName);
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }
                    recipe.ImagePath = "/" + folder + fileName;
                }

                recipe.CreatedDate = DateTime.Now;
                _context.Recipes.Add(recipe);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Product added successfully!";
                return RedirectToAction("Products");
            }
            return View(recipe);
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _context.Recipes.FindAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(Recipe recipe, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                var existing = await _context.Recipes.FindAsync(recipe.Id);
                if (existing == null) return NotFound();

                existing.Name = recipe.Name;
                existing.Category = recipe.Category;
                existing.Ingredients = recipe.Ingredients;
                existing.Procedure = recipe.Procedure;
                existing.IsFree = recipe.IsFree;

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var folder = "images/recipes/";
                    var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageFile.FileName);
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }
                    existing.ImagePath = "/" + folder + fileName;
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Product updated successfully!";
                return RedirectToAction("Products");
            }
            return View(recipe);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Recipes.FindAsync(id);
            if (product != null)
            {
                _context.Recipes.Remove(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Product deleted successfully!";
            }
            return RedirectToAction("Products");
        }
    }

    }