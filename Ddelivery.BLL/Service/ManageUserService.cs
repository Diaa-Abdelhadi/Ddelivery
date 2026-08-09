using Ddelivery.DAL.DTO.Request;
using Ddelivery.DAL.DTO.Response;
using Ddelivery.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ddelivery.BLL.Service
{
    public class ManageUserService : IMangerUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ManageUserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<BaseResponse> BlockedUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new BaseResponse { Success = false, Message = "User not found" };
            }
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            await _userManager.UpdateAsync(user);
            return new BaseResponse
            {
                Success = true,
                Message = "User blocked successfully"
            };
        }

        public async Task<BaseResponse> ChangeUserRoleAsync(ChangeUserRoleRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
            {
                return new BaseResponse { Success = false, Message = "User not found" };
            }
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, request.Role);

            return new BaseResponse
            {
                Success = true,
                Message = "User role changed successfully"
            };
        }

        public async Task<List<UserListResponse>> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = users.Adapt<List<UserListResponse>>();
            for (int i = 0; i < users.Count; i++)
            {
                var roles = await _userManager.GetRolesAsync(users[i]);
                result[i].Roles = new List<string>(roles);
            }

            return result;
        }

        public async Task<BaseResponse> UnBlockedUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new BaseResponse { Success = false, Message = "User not found" };
            }
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, null);
            await _userManager.UpdateAsync(user);
            return new BaseResponse
            {
                Success = true,
                Message = "User unblocked successfully"
            };
        }
    }
}
