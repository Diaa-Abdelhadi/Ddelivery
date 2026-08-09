using Ddelivery.BLL.Service;
using Ddelivery.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ddelivery.PL.Areas.Owner
{
    [Route("api/owner/[controller]")]
    [ApiController]
    [Authorize(Roles = "RestaurantOwner")]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] RestaurantRequest request)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _restaurantService.CreateRestaurant(request, ownerId);
            return Ok(response);
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _restaurantService.GetMyRestaurants(ownerId);
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] RestaurantRequest request)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _restaurantService.UpdateRestaurantAsync(id, request, ownerId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
