using Ddelivery.BLL.Service;
using Ddelivery.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ddelivery.PL.Areas.Driver
{
    [Route("api/driver/orders")]
    [ApiController]
    [Authorize(Roles = "Driver")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("available")]
        public async Task<IActionResult> Available([FromQuery] string lang = "en")
        {
            var response = await _orderService.GetAvailableOrdersForDriverAsync(lang);
            return Ok(response);
        }

        [HttpGet("my")]
        public async Task<IActionResult> MyDeliveries([FromQuery] string lang = "en")
        {
            var driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _orderService.GetMyDeliveriesAsync(driverId, lang);
            return Ok(response);
        }

        [HttpPatch("{orderId}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int orderId, [FromBody] UpdateOrderStatusRequest request)
        {
            var driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _orderService.UpdateOrderStatusForDriverAsync(orderId, request.Status, driverId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
