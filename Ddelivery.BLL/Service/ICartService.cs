using Ddelivery.DAL.DTO.Request;
using Ddelivery.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.BLL.Service
{
    public interface ICartService
    {
        Task<BaseResponse> AddToCartAsync(string userId, AddToCartRequest request);
        Task<CartSummaryResponse> GetUserCartAsync(string userId, string lang = "en");
        Task<BaseResponse> UpdateQuantityAsync(string userId, int mealId, int count);
        Task<BaseResponse> ClearCartAsync(string userId);
        Task<BaseResponse> RemoveFromCartAsync(string userId, int mealId);
    }
}
