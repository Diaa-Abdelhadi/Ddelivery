using Ddelivery.BLL.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ddelivery.PL.Areas.Admin
{
    [Route("api/admin/earnings")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class EarningsController : ControllerBase
    {
        private readonly IEarningsService _earningsService;

        public EarningsController(IEarningsService earningsService)
        {
            _earningsService = earningsService;
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> Calculate([FromQuery] DateTime date)
        {
            var count = await _earningsService.CalculateDailyEarningsAsync(date);
            return Ok(new { Message = $"Calculated earnings for {date:yyyy-MM-dd}", RecordsUpserted = count });
        }
    }
}
