using Ddelivery.BLL.Service;
using Ddelivery.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ddelivery.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var response = await _restaurantService.GetAllRestaurantsForAdmin();
            return Ok(response);
        }

        [HttpPatch("Toggle-status/{id}")]
        public async Task<IActionResult> ToggleStatus([FromRoute] int id)
        {
            var response = await _restaurantService.ToggleStatus(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
