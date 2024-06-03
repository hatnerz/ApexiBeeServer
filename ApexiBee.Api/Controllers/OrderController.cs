using ApexiBee.Application.DTO;
using ApexiBee.Application.Interfaces;
using ApexiBee.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApexiBee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost("status")]
        public async Task<IActionResult> AddStatus([FromBody] string statusName)
        {
            await orderService.CreateNewStatus(statusName);
            return Ok();
        }

        [HttpGet("status/all")]
        public async Task<IActionResult> GetAllStatuses()
        {
            var statuses = await orderService.GetOrderStatuses();
            return Ok(statuses);
        }

        [HttpDelete("status/{orderStatus}")]
        public async Task<IActionResult> DeleteStatus([FromRoute] string orderStatus)
        {
            await orderService.DeleteStatus(orderStatus);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> PlaceNewOrder([FromBody] NewOrderData orderData)
        {
            var order = await orderService.PlaceOrder(orderData);
            return Ok(order);
        }

        [HttpPatch("approve/{orderId}")]
        public async Task<IActionResult> ApproveOrder([FromRoute] Guid orderId)
        {
            var order = await orderService.ApproveOrder(orderId);
            return Ok(order);
        }

        [HttpPatch("complete/{orderId}")]
        public async Task<IActionResult> CompleteOrder([FromRoute] Guid orderId)
        {
            var order = await orderService.CompleteOrder(orderId);
            return Ok(order);
        }

        [HttpPatch("cancel/{orderId}")]
        public async Task<IActionResult> CancelOrder([FromRoute] Guid orderId)
        {
            var order = await orderService.CancelOrder(orderId);
            return Ok(order);
        }

        [HttpGet("user/all/{userId}")]
        public async Task<IActionResult> GetAllUserOrders([FromRoute] Guid userId)
        {
            var userOrders = await orderService.GetUserOrders(userId);
            return Ok(userOrders);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await orderService.GetAllOrders();
            return Ok(orders);
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetAllOrdersWithFilters([FromBody] OrderFilter filters)
        {
            var orders = await orderService.GetOrdersWithFilters(filters);
            return Ok(orders);
        }

    }
}
