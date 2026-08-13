using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ice_Cream_Parlour_Eproject.Data;
using Ice_Cream_Parlour_Eproject.Models;
using Ice_Cream_Parlour_Eproject.Areas.Models;
using System.Security.Claims;
using System.Text.Json;

namespace Ice_Cream_Parlour_Eproject.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Helper to get cart from Session
        private List<CartItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString("ShoppingCart");
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>();
            }
            try
            {
                return JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
            }
            catch
            {
                return new List<CartItem>();
            }
        }

        // Helper to save cart to Session
        private void SaveCart(List<CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("ShoppingCart", cartJson);
        }

        // ===== VIEW CART =====
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        // ===== ADD TO CART =====
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            if (quantity <= 0) quantity = 1;

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                TempData["Error"] = "Product not found.";
                return RedirectToAction(nameof(Index));
            }

            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = quantity,
                    ImagePath = product.ImagePath
                });
            }

            SaveCart(cart);
            TempData["Success"] = $"{product.Name} added to cart!";

            // Redirect back to referring page or to Cart Index
            string? referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }
            return RedirectToAction(nameof(Index));
        }

        // ===== REMOVE FROM CART =====
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
                TempData["Success"] = $"{item.ProductName} removed from cart.";
            }
            return RedirectToAction(nameof(Index));
        }

        // ===== UPDATE QUANTITY =====
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            if (quantity <= 0)
            {
                return RemoveFromCart(productId);
            }

            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                item.Quantity = quantity;
                SaveCart(cart);
                TempData["Success"] = "Cart updated successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        // ===== CHECKOUT (GET) =====
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                TempData["Error"] = "Your cart is empty. Add products before checking out.";
                return RedirectToAction("Product", "Home");
            }

            var model = new OrderCheckoutViewModel();

            // Prefill if authenticated
            if (User.Identity?.IsAuthenticated == true)
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                if (!string.IsNullOrEmpty(userEmail))
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
                    if (user != null)
                    {
                        model.Name = user.FullName ?? string.Empty;
                        model.Email = user.Email ?? string.Empty;
                        model.Phone = user.PhoneNumber ?? string.Empty;
                        model.DeliveryAddress = user.Address ?? string.Empty;
                    }
                }
            }

            ViewBag.Cart = cart;
            return View(model);
        }

        // ===== CHECKOUT (POST) =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(OrderCheckoutViewModel model)
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Product", "Home");
            }

            if (ModelState.IsValid)
            {
                // Find or Create Customer
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email.ToLower() == model.Email.ToLower());
                if (customer == null)
                {
                    customer = new Customer
                    {
                        FullName = model.Name,
                        Email = model.Email,
                        Phone = model.Phone,
                        Address = model.DeliveryAddress,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    };
                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Update contact info if changed
                    customer.FullName = model.Name;
                    customer.Phone = model.Phone;
                    customer.Address = model.DeliveryAddress;
                    await _context.SaveChangesAsync();
                }

                // Place Order
                var order = new Order
                {
                    OrderNumber = "ORD-" + DateTime.Now.ToString("yyyyMMdd") + "-" + new Random().Next(1000, 9999),
                    OrderDate = DateTime.Now,
                    CustomerId = customer.Id,
                    CustomerName = model.Name,
                    CustomerEmail = model.Email,
                    CustomerPhone = model.Phone,
                    DeliveryAddress = model.DeliveryAddress,
                    PaymentMethod = model.PaymentMethod,
                    Notes = model.Notes,
                    OrderStatus = "Pending",
                    PaymentStatus = "Pending",
                    TotalAmount = cart.Sum(item => item.LineTotal)
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Save Order Items
                foreach (var item in cart)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _context.OrderItems.Add(orderItem);

                    // Update product stock if applicable
                    var dbProduct = await _context.Products.FindAsync(item.ProductId);
                    if (dbProduct != null)
                    {
                        dbProduct.StockQuantity = Math.Max(0, dbProduct.StockQuantity - item.Quantity);
                    }
                }

                await _context.SaveChangesAsync();

                // Clear session cart
                HttpContext.Session.Remove("ShoppingCart");

                TempData["Success"] = "Order placed successfully!";
                return RedirectToAction(nameof(Confirmation), new { id = order.Id });
            }

            ViewBag.Cart = cart;
            return View(model);
        }

        // ===== ORDER CONFIRMATION =====
        public async Task<IActionResult> Confirmation(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
