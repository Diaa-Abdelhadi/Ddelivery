using Ddelivery.BLL.Service;
using Ddelivery.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ddelivery.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class CheckoutsController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public CheckoutsController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _orderService.CheckoutAsync(userId, request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetMyOrders([FromQuery] string lang = "en")
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _orderService.GetMyOrdersAsync(userId, lang);
            return Ok(response);
        }

        [HttpGet("orders/{orderId}")]
        public async Task<IActionResult> GetOrderDetails([FromRoute] int orderId, [FromQuery] string lang = "en")
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _orderService.GetOrderDetailsAsync(userId, orderId, lang);
            if (response is null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}
