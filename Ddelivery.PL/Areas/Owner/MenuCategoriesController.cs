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
    public class MenuCategoriesController : ControllerBase
    {
        private readonly IMenuCategoryService _menuCategoryService;

        public MenuCategoriesController(IMenuCategoryService menuCategoryService)
        {
            _menuCategoryService = menuCategoryService;
        }

        [HttpPost("restaurants/{restaurantId}/menu-categories")]
        public async Task<IActionResult> Create([FromRoute] int restaurantId, [FromBody] MenuCategoryRequest request)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _menuCategoryService.CreateMenuCategory(request, restaurantId, ownerId);
            if (response is null)
            {
                return Forbid();
            }
            return Ok(response);
        }

        [HttpGet("restaurants/{restaurantId}/menu-categories")]
        public async Task<IActionResult> Index([FromRoute] int restaurantId)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _menuCategoryService.GetMenuCategoriesForOwner(restaurantId, ownerId);
            if (response is null)
            {
                return Forbid();
            }
            return Ok(response);
        }

        [HttpPatch("menu-categories/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] MenuCategoryRequest request)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _menuCategoryService.UpdateMenuCategoryAsync(id, request, ownerId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("menu-categories/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _menuCategoryService.DeleteMenuCategoryAsync(id, ownerId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPatch("menu-categories/Toggle-status/{id}")]
        public async Task<IActionResult> ToggleStatus([FromRoute] int id)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _menuCategoryService.ToggleStatus(id, ownerId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
