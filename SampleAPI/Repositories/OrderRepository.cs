using Microsoft.EntityFrameworkCore;
using SampleAPI.Entities;
using SampleAPI.Requests;

namespace SampleAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SampleApiDbContext _sampleApiDbContext;

        public OrderRepository(SampleApiDbContext sampleApiDbContext)
        {
            _sampleApiDbContext = sampleApiDbContext;
        }

        public async Task<List<Order>> GetRecentOrders()
        {
            try
            {
                DateTime oneDayAgo = DateTime.Now.AddDays(-1);

                var recentOrders = await _sampleApiDbContext.Orders
                    .Where(o => o.CreatedDate >= oneDayAgo && !o.IsDeleted)
                    .ToListAsync();


                return recentOrders;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Order> AddNewOrder(CreateOrderRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Name))
                    throw new ArgumentNullException(nameof(request.Name), "Name cannot be null or empty");

                Order order = new Order
                {
                    Name = request.Name,
                    Description = request.Description,
                    IsInvoiced = request.IsInvoiced,
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                };
                await _sampleApiDbContext.Orders.AddAsync(order);
                await _sampleApiDbContext.SaveChangesAsync();

                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
