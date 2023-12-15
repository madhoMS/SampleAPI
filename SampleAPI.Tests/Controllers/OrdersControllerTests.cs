using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SampleAPI.Controllers;
using SampleAPI.Entities;
using SampleAPI.Repositories;
using SampleAPI.Requests;
using System.Net;

namespace SampleAPI.Tests.Controllers
{
    public class OrdersControllerTests
    {

        [Fact]
        public async Task PostOrder_ReturnsOkResult()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var controller = new OrdersController(orderRepositoryMock.Object);

            var createOrderRequest = new CreateOrderRequest
            {
                Name = "TestOrder",
                Description = "TestDescription"
            };

            var expectedResponse = new Order
            {
                Id = Guid.NewGuid(),
                Name = "TestOrder",
                Description = "TestDescription"
            };

            orderRepositoryMock.Setup(x => x.AddNewOrder(It.IsAny<CreateOrderRequest>())).ReturnsAsync(expectedResponse);

            // Act
            var result = await controller.PostOrder(createOrderRequest);

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;

            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var response = okResult.Value.Should().BeAssignableTo<Order>().Subject;
            // Validate properties of the returned Order object
            response.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task PostOrder_ReturnsBadRequest()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var controller = new OrdersController(orderRepositoryMock.Object);

            var createOrderRequest = new CreateOrderRequest
            {
                Name = "",
                Description = "TestDescription"
            };

            orderRepositoryMock.Setup(x => x.AddNewOrder(It.IsAny<CreateOrderRequest>())).ReturnsAsync((Order)null);

            // Act
            var result = await controller.PostOrder(createOrderRequest);

            // Assert
            result.Should().NotBeNull().And.BeOfType<ObjectResult>();
            var objectResult = (ObjectResult)result;

            objectResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            objectResult.Value.Should().Be("Failed to insert record!");
        }
        [Fact]
        public async Task GetOrders_ShouldReturnOkWithMessage_WhenRecordsExist()
        {
            // Arrange
            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock.Setup(x => x.GetRecentOrders()).ReturnsAsync(new List<Order>
            {
                new Order { Id = Guid.NewGuid(), Name = "Order1", Description = "Description1" },
                new Order { Id = Guid.NewGuid(), Name = "Order2", Description = "Description2" }
            });

            var controller = new OrdersController(orderRepositoryMock.Object);

            // Act
            var actionResult = await controller.GetOrders();

            // Assert
            var result = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Records Fetch Successfully!", result.Value);
        }

        [Fact]
        public async Task GetOrders_ShouldReturnNoContentWithMessage_WhenNoRecordsExist()
        {
            // Arrange
            var orderRepository = new Mock<IOrderRepository>();
            orderRepository.Setup(repo => repo.GetRecentOrders()).ReturnsAsync(new List<Order>());
             
            var controller = new OrdersController(orderRepository.Object);

            // Act
            var actionResult = await controller.GetOrders();

            // Assert
            var result = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.NoContent, result.StatusCode);
            Assert.Equal("No Data Found!", result.Value);
        }


    }
}
