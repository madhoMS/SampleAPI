using SampleAPI.Entities;
using SampleAPI.Requests;

namespace SampleAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetRecentOrders();
        Task<Order> AddNewOrder(CreateOrderRequest request);
    }
}
