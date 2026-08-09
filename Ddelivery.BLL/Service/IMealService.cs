using Ddelivery.DAL.DTO.Request;
using Ddelivery.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.BLL.Service
{
    public interface IMealService
    {
        Task<MealResponse?> CreateMeal(MealRequest request, int restaurantId, string ownerId);
        Task<PagintedResponse<MealResponse>?> GetMealsForOwner(int restaurantId, string ownerId, int page = 1, int limit = 10);
        Task<PagintedResponse<MealUserResponse>> GetAllMealsForUserAsync(
            int restaurantId, string lang = "en", int page = 1, int limit = 10, string? search = null,
            int? menuCategoryId = null, string? sortBy = null, bool asc = true);
        Task<MealUserDetails> GetMealDetailsForUser(int id, string lang = "en");
        Task<BaseResponse> UpdateMealAsync(int id, UpdateMealRequest request, string ownerId);
        Task<BaseResponse> ToggleStatus(int id, string ownerId);
    }
}
