using Microsoft.EntityFrameworkCore;
using Ice_Cream_Parlour_Eproject.Data;
using Ice_Cream_Parlour_Eproject.Services.Interfaces;
using Ice_Cream_Parlour_Eproject.Areas.Models;
using Ice_Cream_Parlour_Eproject.Areas.Models.ViewModels;

namespace Ice_Cream_Parlour_Eproject.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerListViewModel> GetPagedAsync(string? search, string sortBy, string sortDir, int page, int pageSize)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(c => c.FullName.Contains(search) || c.Email.Contains(search));

            query = sortBy switch
            {
                "email" => sortDir == "asc" ? query.OrderBy(c => c.Email) : query.OrderByDescending(c => c.Email),
                "date" => sortDir == "asc" ? query.OrderBy(c => c.CreatedAt) : query.OrderByDescending(c => c.CreatedAt),
                _ => sortDir == "asc" ? query.OrderBy(c => c.FullName) : query.OrderByDescending(c => c.FullName)
            };

            var total = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CustomerViewModel
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    Email = c.Email,
                    Phone = c.Phone,
                    Address = c.Address,
                    ProfileImagePath = c.ProfileImagePath,
                    IsActive = c.IsActive,
                    CreatedAt = c.CreatedAt,
                    TotalOrders = c.Orders.Count,
                    TotalSpent = c.Orders.Sum(o => o.TotalAmount)
                })
                .ToListAsync();

            return new CustomerListViewModel
            {
                Customers = new PaginatedList<CustomerViewModel>
                {
                    Items = items,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total
                },
                SearchTerm = search,
                SortBy = sortBy,
                SortDir = sortDir,
                TotalCustomers = total
            };
        }

        public async Task<CustomerViewModel?> GetByIdAsync(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null) return null;

            return new CustomerViewModel
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                ProfileImagePath = customer.ProfileImagePath,
                IsActive = customer.IsActive,
                CreatedAt = customer.CreatedAt,
                TotalOrders = customer.Orders.Count,
                TotalSpent = customer.Orders.Sum(o => o.TotalAmount)
            };
        }

        public async Task<bool> CreateAsync(CustomerViewModel model, IWebHostEnvironment env)
        {
            if (await _context.Customers.AnyAsync(c => c.Email == model.Email))
                return false;

            var customer = new Customer
            {
                FullName = model.FullName,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                IsActive = model.IsActive,
                CreatedAt = DateTime.Now
            };

            if (model.ProfileImage != null)
            {
                var folder = "images/customers/";
                var fileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                var fullPath = Path.Combine(env.WebRootPath, folder, fileName);
                Directory.CreateDirectory(Path.Combine(env.WebRootPath, folder));
                using var stream = new FileStream(fullPath, FileMode.Create);
                await model.ProfileImage.CopyToAsync(stream);
                customer.ProfileImagePath = "/" + folder + fileName;
            }

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(CustomerViewModel model, IWebHostEnvironment env)
        {
            var customer = await _context.Customers.FindAsync(model.Id);
            if (customer == null) return false;

            if (await _context.Customers.AnyAsync(c => c.Email == model.Email && c.Id != model.Id))
                return false;

            customer.FullName = model.FullName;
            customer.Email = model.Email;
            customer.Phone = model.Phone;
            customer.Address = model.Address;
            customer.IsActive = model.IsActive;

            if (model.ProfileImage != null)
            {
                if (!string.IsNullOrEmpty(customer.ProfileImagePath))
                {
                    var oldPath = Path.Combine(env.WebRootPath, customer.ProfileImagePath.TrimStart('/'));
                    if (File.Exists(oldPath)) File.Delete(oldPath);
                }

                var folder = "images/customers/";
                var fileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                var fullPath = Path.Combine(env.WebRootPath, folder, fileName);
                Directory.CreateDirectory(Path.Combine(env.WebRootPath, folder));
                using var stream = new FileStream(fullPath, FileMode.Create);
                await model.ProfileImage.CopyToAsync(stream);
                customer.ProfileImagePath = "/" + folder + fileName;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, IWebHostEnvironment env)
        {
            var customer = await _context.Customers
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null) return false;
            if (customer.Orders.Any()) return false;

            if (!string.IsNullOrEmpty(customer.ProfileImagePath))
            {
                var oldPath = Path.Combine(env.WebRootPath, customer.ProfileImagePath.TrimStart('/'));
                if (File.Exists(oldPath)) File.Delete(oldPath);
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Order>> GetOrderHistoryAsync(int customerId)
        {
            return await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
    }
}