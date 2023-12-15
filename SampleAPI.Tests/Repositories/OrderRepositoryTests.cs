using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SampleAPI.Entities;
using SampleAPI.Repositories;
using SampleAPI.Requests;

namespace SampleAPI.Tests.Repositories
{
    public class OrderRepositoryTests
    {
        [Fact]
        public async Task GetRecentOrders_ShouldReturnRecentOrders()
        {
            // Arrange
            var ordersData = new List<CreateOrderRequest>
            {
                new CreateOrderRequest { Name = "Order1", Description = "Description1",IsInvoiced = true },
                new CreateOrderRequest { Name = "Order2", Description = "Description2",IsInvoiced = true },
                new CreateOrderRequest { Name = "Order3", Description = "Description3",IsInvoiced = true },
                new CreateOrderRequest { Name = "Order4", Description = "Description4",IsInvoiced = true },
            };

            var mockDbContext = MockSampleApiDbContextFactory.GenerateMockContext();
            var orderRepository = new OrderRepository(mockDbContext);

            foreach (var orderRequest in ordersData)
                await orderRepository.AddNewOrder(orderRequest);

            // Act
            var result = await orderRepository.GetRecentOrders();

            // Assert
            Assert.Equal(4, result.Count); // Adjust the expected count based on your test data
            Assert.All(result, o => Assert.False(o.IsDeleted));
        }

        [Fact]
        public async Task AddNewOrder_WithValidData_ShouldAddOrder()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                Name = "TestOrder",
                Description = "TestDescription",
                IsInvoiced = true,
            };

            var mockDbContext = MockSampleApiDbContextFactory.GenerateMockContext();
            var orderRepository = new OrderRepository(mockDbContext);

            // Act
            var result = await orderRepository.AddNewOrder(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Name, result.Name);
            Assert.Equal(request.Description, result.Description);
            Assert.Equal(request.IsInvoiced, result.IsInvoiced);
            Assert.False(result.IsDeleted);
        }

    }
}