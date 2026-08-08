using Microsoft.EntityFrameworkCore;
using Ice_Cream_Parlour_Eproject.Data;
using Ice_Cream_Parlour_Eproject.Areas.Models;
using Ice_Cream_Parlour_Eproject.Areas.Models.ViewModels;
using Ice_Cream_Parlour_Eproject.Areas.Models.Enums;

namespace Ice_Cream_Parlour_Eproject.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderListViewModel> GetPagedAsync(string? search, OrderStatus? status, int page, int pageSize)
        {
            var query = _context.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(o => o.OrderNumber.Contains(search) || o.CustomerName.Contains(search));

            if (status.HasValue)
                query = query.Where(o => o.OrderStatus == status.Value.ToString());

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize)
                .Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    OrderDate = o.OrderDate,
                    CustomerName = o.CustomerName,
                    TotalAmount = o.TotalAmount,
                    PaymentMethod = o.PaymentMethod,
                    PaymentStatus = o.PaymentStatus,
                    OrderStatus = o.OrderStatus,
                    Notes = o.Notes
                })
                .ToListAsync();

            return new OrderListViewModel
            {
                Orders = new PaginatedList<OrderViewModel> { Items = items, Page = page, PageSize = pageSize, TotalCount = total },
                SearchTerm = search,
                StatusFilter = status
            };
        }

        public async Task<OrderViewModel?> GetByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return null;

            var subTotal = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);
            var taxAmount = subTotal * 0.10m;

            return new OrderViewModel
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                CustomerId = order.CustomerId,
                CustomerName = order.CustomerName,
                SubTotal = subTotal,
                TaxAmount = taxAmount,
                TotalAmount = order.TotalAmount,
                PaymentMethod = order.PaymentMethod,
                PaymentStatus = order.PaymentStatus,
                OrderStatus = order.OrderStatus,
                Notes = order.Notes,
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.ProductName,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    LineTotal = oi.Quantity * oi.UnitPrice
                }).ToList(),
                History = new List<OrderHistoryDto>
                {
                    new OrderHistoryDto
                    {
                        Status = order.OrderStatus,
                        Notes = order.Notes,
                        ChangedAt = order.OrderDate,
                        ChangedBy = "Admin"
                    }
                }
            };
        }

        public async Task<OrderCreateViewModel> GetCreateModelAsync()
        {
            return new OrderCreateViewModel
            {
                Customers = await _context.Customers.Select(c => new CustomerOrderSelectDto
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    Email = c.Email
                }).ToListAsync(),
                Products = await _context.Products.Select(p => new ProductOrderSelectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    FinalPrice = p.Price,
                    StockQuantity = p.StockQuantity
                }).ToListAsync()
            };
        }

        public async Task<int> CreateAsync(OrderCreateViewModel model, string? userId)
        {
            var customer = await _context.Customers.FindAsync(model.CustomerId);
            if (customer == null) return 0;

            var order = new Areas.Models.Order
            {
                CustomerId = model.CustomerId,
                CustomerName = customer.FullName,
                CustomerEmail = customer.Email,
                CustomerPhone = customer.Phone,
                DeliveryAddress = customer.Address,
                OrderNumber = "ORD-" + DateTime.Now.ToString("yyyyMMdd") + "-" + new Random().Next(1000, 9999),
                OrderDate = DateTime.Now,
                PaymentMethod = model.PaymentMethod,
                PaymentStatus = "Pending",
                OrderStatus = "Pending",
                Notes = model.Notes,
                TotalAmount = 0
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            if (model.Lines != null && model.Lines.Any())
            {
                foreach (var line in model.Lines)
                {
                    var product = await _context.Products.FindAsync(line.ProductId);
                    if (product != null)
                    {
                        var item = new OrderItem
                        {
                            OrderId = order.Id,
                            ProductId = product.Id,
                            ProductName = product.Name,
                            Quantity = line.Quantity,
                            UnitPrice = product.Price
                        };
                        orderItems.Add(item);
                        totalAmount += line.Quantity * product.Price;
                    }
                }
            }

            if (orderItems.Any())
            {
                _context.OrderItems.AddRange(orderItems);
                order.TotalAmount = totalAmount;
                await _context.SaveChangesAsync();
                return order.Id;
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return 0;
        }

        public async Task UpdateStatusAsync(int id, OrderStatus orderStatus, PaymentStatus paymentStatus, string? userId, string? notes)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.OrderStatus = orderStatus.ToString();
                order.PaymentStatus = paymentStatus.ToString();
                await _context.SaveChangesAsync();
            }
        }
    }
}