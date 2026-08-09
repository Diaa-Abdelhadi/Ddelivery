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
    public class MealsController : ControllerBase
    {
        private readonly IMealService _mealService;

        public MealsController(IMealService mealService)
        {
            _mealService = mealService;
        }

        [HttpPost("restaurants/{restaurantId}/meals")]
        public async Task<IActionResult> Create([FromRoute] int restaurantId, [FromForm] MealRequest request)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _mealService.CreateMeal(request, restaurantId, ownerId);
            if (response is null)
            {
                return Forbid();
            }
            return Ok(response);
        }

        [HttpGet("restaurants/{restaurantId}/meals")]
        public async Task<IActionResult> Index([FromRoute] int restaurantId, [FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _mealService.GetMealsForOwner(restaurantId, ownerId, page, limit);
            if (response is null)
            {
                return Forbid();
            }
            return Ok(response);
        }

        [HttpPatch("meals/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] UpdateMealRequest request)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _mealService.UpdateMealAsync(id, request, ownerId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPatch("meals/Toggle-status/{id}")]
        public async Task<IActionResult> ToggleStatus([FromRoute] int id)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _mealService.ToggleStatus(id, ownerId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
