using Ddelivery.BLL.Service;
using Ddelivery.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ddelivery.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MangersController : ControllerBase
    {
        private readonly IMangerUserService _mangerUser;

        public MangersController(IMangerUserService mangerUser)
        {
            _mangerUser = mangerUser;
        }
        [HttpGet("users")]
        public async Task<ActionResult> GetUsers()
        {
            var users = await _mangerUser.GetUsersAsync();
            return Ok(users);
        }
        [HttpPatch("block/{userId}")]
        public async Task<ActionResult> BlockUser([FromRoute] string userId)
        {
            var response = await _mangerUser.BlockedUserAsync(userId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPatch("unblock/{userId}")]
        public async Task<ActionResult> UnBlockUser([FromRoute] string userId)
        {
            var response = await _mangerUser.UnBlockedUserAsync(userId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPatch("change-role")]
        public async Task<ActionResult> ChangeUserRole([FromBody] ChangeUserRoleRequest request)
        {
            var response = await _mangerUser.ChangeUserRoleAsync(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
