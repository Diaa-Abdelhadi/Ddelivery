using Ddelivery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Repository
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order request);
        Task<Order> GetOrderBySessionIdAsync(string sessionId);
        Task<Order> UpdateAsync(Order order);
        Task<List<Order>> GetByUserIdAsync(string userId);
        Task<Order?> FindByIdAsync(int id);
        Task<(List<Order> Items, int TotalCount)> GetByRestaurantIdAsync(int restaurantId, int page, int limit);
        Task<List<Order>> GetAvailableForDriverAsync();
        Task<List<Order>> GetByDriverIdAsync(string driverId);
        Task<List<Order>> GetStalePendingOrdersAsync(DateTime cutoff);
        Task<List<Order>> GetDeliveredOrdersByDateAsync(DateTime date);
        Task<bool> HasUserDeliveredOrderForMeal(string userId, int mealId);
    }
}
