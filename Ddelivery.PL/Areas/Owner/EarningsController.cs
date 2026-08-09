using Ddelivery.BLL.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ddelivery.PL.Areas.Owner
{
    [Route("api/owner")]
    [ApiController]
    [Authorize(Roles = "RestaurantOwner")]
    public class EarningsController : ControllerBase
    {
        private readonly IEarningsService _earningsService;

        public EarningsController(IEarningsService earningsService)
        {
            _earningsService = earningsService;
        }

        [HttpGet("restaurants/{restaurantId}/earnings")]
        public async Task<IActionResult> GetEarnings([FromRoute] int restaurantId, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _earningsService.GetRestaurantEarningsAsync(restaurantId, ownerId, fromDate, toDate);
            if (response is null)
            {
                return Forbid();
            }
            return Ok(response);
        }
    }
}
