using Ddelivery.DAL.DTO.Request;
using Ddelivery.DAL.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ddelivery.BLL.Service
{
    public interface IMangerUserService
    {
        Task<List<UserListResponse>> GetUsersAsync();
        Task<BaseResponse> BlockedUserAsync(string userId);
        Task<BaseResponse> UnBlockedUserAsync(string userId);
        Task<BaseResponse> ChangeUserRoleAsync(ChangeUserRoleRequest request);
    }
}
