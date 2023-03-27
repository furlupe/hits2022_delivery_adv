using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DeliveryDeck_Backend.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        [HttpGet("active")]
        public Task<IActionResult> GetActiveOrder()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task<IActionResult> CreateOrder()
        {
            throw new NotImplementedException();
        }

        [HttpPatch("{orderId}/change_status")]
        public Task<IActionResult> CancelUserActiveOrder(Guid orderId, [FromQuery, BindRequired] string status)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{orderId}/repeat")]
        public Task<IActionResult> RepeatOrder([BindRequired] Guid orderId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public Task<IActionResult> GetOrders([FromQuery] int orderNumber, [FromQuery] List<string> filters)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{orderId}")]
        public Task<IActionResult> GetOrderById([BindRequired] Guid orderId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("deliverable_orders")]
        public Task<IActionResult> GetOrdersForDelivery()
        {
            throw new NotImplementedException();
        }
    }
}
