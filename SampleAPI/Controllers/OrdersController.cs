using Microsoft.AspNetCore.Mvc;
using SampleAPI.Repositories;
using SampleAPI.Requests;
using System.Net;

namespace SampleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetOrders()
        {
            try
            {
                var response = await _orderRepository.GetRecentOrders();
                if (response != null && response.Count > 0)
                    return StatusCode((int)HttpStatusCode.OK, "Records Fetch Successfully!");
                else
                    return StatusCode((int)HttpStatusCode.NoContent, "No Data Found!");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                var response = await _orderRepository.AddNewOrder(request);
                if (response != null)
                    return Ok(response);
                else
                    return StatusCode((int)HttpStatusCode.BadRequest, "Failed to insert record!");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
