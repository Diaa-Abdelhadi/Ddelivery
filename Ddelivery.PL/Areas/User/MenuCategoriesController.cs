using Ddelivery.BLL.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ddelivery.PL.Areas.User
{
    [Route("api/restaurants/{restaurantId}/menu-categories")]
    [ApiController]
    public class MenuCategoriesController : ControllerBase
    {
        private readonly IMenuCategoryService _menuCategoryService;

        public MenuCategoriesController(IMenuCategoryService menuCategoryService)
        {
            _menuCategoryService = menuCategoryService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index([FromRoute] int restaurantId, [FromQuery] string lang = "en")
        {
            var response = await _menuCategoryService.GetMenuCategoriesForUser(restaurantId, lang);
            return Ok(response);
        }
    }
}
