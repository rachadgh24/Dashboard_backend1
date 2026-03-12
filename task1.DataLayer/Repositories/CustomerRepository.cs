using Microsoft.EntityFrameworkCore;
using task1.DataLayer.DbContexts;
using task1.DataLayer.Entities;
using task1.DataLayer.Interfaces;

namespace task1.DataLayer.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DotNetTrainingCoreContext _context;

        public CustomerRepository(DotNetTrainingCoreContext context)
        {
            _context = context;
        }

        public IQueryable<Customer> GetAll()
        {
            return _context.Customers.AsNoTracking();
        }

        public IQueryable<Customer> GetById(int id)
        {
            return _context.Customers.Where(c => c.Id == id);
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }

        public Task<Customer> AddCustomerAsync(Customer customer)
        {
            var entity = _context.Customers.Add(customer);
            return Task.FromResult(entity.Entity);
        }

        public async Task<Customer?> UpdateCustomer(int id, Customer customer)
        {
            var entity = await _context.Customers.FindAsync(id);
            if (entity == null) return null;

            entity.Name = customer.Name;
            entity.LastName = customer.LastName;
            entity.City = customer.City;
            entity.Email = customer.Email;
            return entity;
        }

        public async Task<bool> DeleteCustomer(int id)
        {
            var entity = await _context.Customers.FindAsync(id);
            if (entity == null) return false;

            var relatedCars = _context.Cars.Where(c => c.CustomerId == id);
            _context.Cars.RemoveRange(relatedCars);
            _context.Customers.Remove(entity);
            return true;
        }

        public async Task<List<Customer>> Search(string? query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new List<Customer>();
            }

            var lowerQuery = query.Trim().ToLower();
            return await _context.Customers
                .Where(c => (c.Name != null && c.Name.ToLower().Contains(lowerQuery))
                         || (c.LastName != null && c.LastName.ToLower().Contains(lowerQuery))
                         || (c.City != null && c.City.ToLower().Contains(lowerQuery)))
                .ToListAsync();
        }
        public async Task<List<Customer>> PaginateCustomers(int page){
            return await _context.Customers
                .OrderBy(c => c.Id)
                .Skip((page - 1) * 4)
                .Take(4)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Customers.CountAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
