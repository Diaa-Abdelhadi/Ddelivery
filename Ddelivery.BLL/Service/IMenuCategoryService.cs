using Ddelivery.DAL.DTO.Request;
using Ddelivery.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.BLL.Service
{
    public interface IMenuCategoryService
    {
        Task<MenuCategoryResponse?> CreateMenuCategory(MenuCategoryRequest request, int restaurantId, string ownerId);
        Task<List<MenuCategoryResponse>?> GetMenuCategoriesForOwner(int restaurantId, string ownerId);
        Task<List<MenuCategoryUserResponse>> GetMenuCategoriesForUser(int restaurantId, string lang = "en");
        Task<BaseResponse> UpdateMenuCategoryAsync(int id, MenuCategoryRequest request, string ownerId);
        Task<BaseResponse> DeleteMenuCategoryAsync(int id, string ownerId);
        Task<BaseResponse> ToggleStatus(int id, string ownerId);
    }
}
