using Ddelivery.BLL.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ddelivery.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(
            [FromQuery] string lang = "en",
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool asc = true)
        {
            var response = await _restaurantService.GetAllRestaurantsForUserAsync(lang, page, limit, search, sortBy, asc);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details([FromRoute] int id, [FromQuery] string lang = "en")
        {
            var response = await _restaurantService.GetRestaurantDetailsForUser(id, lang);
            return Ok(response);
        }
    }
}
