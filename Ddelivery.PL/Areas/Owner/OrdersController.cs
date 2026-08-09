using Ddelivery.BLL.Service;
using Ddelivery.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ddelivery.PL.Areas.Owner
{
    [Route("api/owner")]
    [ApiController]
    [Authorize(Roles = "RestaurantOwner")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("restaurants/{restaurantId}/orders")]
        public async Task<IActionResult> Index([FromRoute] int restaurantId, [FromQuery] string lang = "en", [FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _orderService.GetOrdersForOwnerAsync(restaurantId, ownerId, lang, page, limit);
            if (response is null)
            {
                return Forbid();
            }
            return Ok(response);
        }

        [HttpPatch("orders/{orderId}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int orderId, [FromBody] UpdateOrderStatusRequest request)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _orderService.UpdateOrderStatusForOwnerAsync(orderId, request.Status, ownerId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
