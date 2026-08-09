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
    public class RestaurantEarningsRepository : IRestaurantEarningsRepository
    {
        private readonly ApplicationDbContext _context;

        public RestaurantEarningsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveAllAsync(List<RestaurantEarnings> earnings)
        {
            if (earnings.Count == 0)
            {
                return;
            }

            var date = earnings[0].Date;
            var restaurantIds = earnings.Select(e => e.RestaurantId).ToList();

            var existing = await _context.RestaurantEarnings
                .Where(e => e.Date == date && restaurantIds.Contains(e.RestaurantId))
                .ToListAsync();
            var existingByRestaurantId = existing.ToDictionary(e => e.RestaurantId);

            var toInsert = new List<RestaurantEarnings>();

            foreach (var item in earnings)
            {
                if (existingByRestaurantId.TryGetValue(item.RestaurantId, out var existingRecord))
                {
                    existingRecord.TotalRevenue = item.TotalRevenue;
                    existingRecord.OrderCount = item.OrderCount;
                }
                else
                {
                    toInsert.Add(item);
                }
            }

            if (toInsert.Count > 0)
            {
                await _context.RestaurantEarnings.AddRangeAsync(toInsert);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<RestaurantEarnings>> GetByRestaurantIdAsync(int restaurantId, DateTime fromDate, DateTime toDate)
        {
            return await _context.RestaurantEarnings
                .Where(e => e.RestaurantId == restaurantId && e.Date >= fromDate && e.Date <= toDate)
                .OrderBy(e => e.Date)
                .ToListAsync();
        }
    }

}
