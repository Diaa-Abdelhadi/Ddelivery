using Ddelivery.BLL.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ddelivery.PL.Areas.Driver
{
    [Route("api/driver/earnings")]
    [ApiController]
    [Authorize(Roles = "Driver")]
    public class EarningsController : ControllerBase
    {
        private readonly IEarningsService _earningsService;

        public EarningsController(IEarningsService earningsService)
        {
            _earningsService = earningsService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetEarnings([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            var driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _earningsService.GetDriverEarningsAsync(driverId, fromDate, toDate);
            return Ok(response);
        }
    }
}
