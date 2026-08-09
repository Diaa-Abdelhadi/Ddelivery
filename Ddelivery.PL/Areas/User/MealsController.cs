using Ddelivery.BLL.Service;
using Ddelivery.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ddelivery.PL.Areas.User
{
    [Route("api/restaurants/{restaurantId}/meals")]
    [ApiController]
    public class MealsController : ControllerBase
    {
        private readonly IMealService _mealService;
        private readonly IReviewService _reviewService;

        public MealsController(IMealService mealService, IReviewService reviewService)
        {
            _mealService = mealService;
            _reviewService = reviewService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(
            [FromRoute] int restaurantId,
            [FromQuery] string lang = "en",
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? search = null,
            [FromQuery] int? menuCategoryId = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool asc = true)
        {
            var response = await _mealService.GetAllMealsForUserAsync(restaurantId, lang, page, limit, search, menuCategoryId, sortBy, asc);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details([FromRoute] int restaurantId, [FromRoute] int id, [FromQuery] string lang = "en")
        {
            var response = await _mealService.GetMealDetailsForUser(id, lang);
            return Ok(response);
        }

        [HttpPost("{id}/reviews")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddReview([FromRoute] int restaurantId, [FromRoute] int id, [FromBody] CreateReviewRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _reviewService.AddReviewAsync(userId, request, id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
