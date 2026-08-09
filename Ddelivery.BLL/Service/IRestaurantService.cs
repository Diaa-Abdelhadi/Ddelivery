using Ddelivery.DAL.DTO.Request;
using Ddelivery.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.BLL.Service
{
    public interface IRestaurantService
    {
        Task<RestaurantResponse> CreateRestaurant(RestaurantRequest request, string ownerId);
        Task<List<RestaurantResponse>> GetAllRestaurantsForAdmin();
        Task<List<RestaurantResponse>> GetMyRestaurants(string ownerId);
        Task<PagintedResponse<RestaurantUserResponse>> GetAllRestaurantsForUserAsync(
            string lang = "en", int page = 1, int limit = 10, string? search = null, string? sortBy = null, bool asc = true);
        Task<RestaurantUserDetails> GetRestaurantDetailsForUser(int id, string lang = "en");
        Task<BaseResponse> UpdateRestaurantAsync(int id, RestaurantRequest request, string ownerId);
        Task<BaseResponse> ToggleStatus(int id);
    }
}
