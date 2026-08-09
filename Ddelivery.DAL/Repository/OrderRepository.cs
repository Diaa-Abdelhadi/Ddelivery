using Ddelivery.DAL.Data;
using Ddelivery.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Order> CreateAsync(Order request)
        {
            await _context.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<Order> GetOrderBySessionIdAsync(string sessionId)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.SessionId == sessionId);
        }
        public async Task<Order> UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }
        public async Task<List<Order>> GetByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Meal).ThenInclude(m => m.Translations)
                .ToListAsync();
        }

        public async Task<Order?> FindByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Meal).ThenInclude(m => m.Translations)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<(List<Order> Items, int TotalCount)> GetByRestaurantIdAsync(int restaurantId, int page, int limit)
        {
            var query = _context.Orders.Where(o => o.RestaurantId == restaurantId);
            var totalCount = await query.CountAsync();
            var items = await query
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Meal).ThenInclude(m => m.Translations)
                .Skip((page - 1) * limit).Take(limit)
                .ToListAsync();
            return (items, totalCount);
        }

        public async Task<List<Order>> GetAvailableForDriverAsync()
        {
            return await _context.Orders
                .Where(o => o.OrderStatus == OrderStatus.Preparing && o.DriverId == null)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Meal).ThenInclude(m => m.Translations)
                .ToListAsync();
        }

        public async Task<List<Order>> GetByDriverIdAsync(string driverId)
        {
            return await _context.Orders
                .Where(o => o.DriverId == driverId)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Meal).ThenInclude(m => m.Translations)
                .ToListAsync();
        }
        public async Task<List<Order>> GetStalePendingOrdersAsync(DateTime cutoff)
        {
            return await _context.Orders
                .Where(o => o.OrderStatus == OrderStatus.Pending && o.OrderTime <= cutoff)
                .Include(o => o.OrderItems)
                .ToListAsync();
        }
        public async Task<List<Order>> GetDeliveredOrdersByDateAsync(DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);
            return await _context.Orders
                .Where(o => o.OrderStatus == OrderStatus.Delivered && o.DeliveredAt >= startOfDay && o.DeliveredAt < endOfDay)
                .Include(o => o.OrderItems)
                .ToListAsync();
        }
        public async Task<bool> HasUserDeliveredOrderForMeal(string userId, int mealId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId && o.OrderStatus == OrderStatus.Delivered)
                .SelectMany(o => o.OrderItems)
                .AnyAsync(oi => oi.MealId == mealId);
        }
    }
}
