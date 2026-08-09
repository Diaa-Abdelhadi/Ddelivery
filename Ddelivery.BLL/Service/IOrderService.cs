using Ddelivery.DAL.DTO.Request;
using Ddelivery.DAL.DTO.Response;
using Ddelivery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.BLL.Service
{
    public interface IOrderService
    {
        Task<CheckoutResponse> CheckoutAsync(string userId, CheckoutRequest request);
        Task<List<OrderResponse>> GetMyOrdersAsync(string userId, string lang = "en");
        Task<OrderResponse?> GetOrderDetailsAsync(string userId, int orderId, string lang = "en");
        Task<PagintedResponse<OrderResponse>?> GetOrdersForOwnerAsync(int restaurantId, string ownerId, string lang = "en", int page = 1, int limit = 10);
        Task<BaseResponse> UpdateOrderStatusForOwnerAsync(int orderId, OrderStatus newStatus, string ownerId);
        Task<List<OrderResponse>> GetAvailableOrdersForDriverAsync(string lang = "en");
        Task<BaseResponse> UpdateOrderStatusForDriverAsync(int orderId, OrderStatus newStatus, string driverId);
        Task<List<OrderResponse>> GetMyDeliveriesAsync(string driverId, string lang = "en");
        Task<int> CancelStaleOrdersAsync(int abandonThresholdMinutes);
    }
}