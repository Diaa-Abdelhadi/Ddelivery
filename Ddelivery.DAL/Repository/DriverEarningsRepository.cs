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
    public class DriverEarningsRepository : IDriverEarningsRepository
    {
        private readonly ApplicationDbContext _context;

        public DriverEarningsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveAllAsync(List<DriverEarnings> earnings)
        {
            if (earnings.Count == 0)
            {
                return;
            }

            var date = earnings[0].Date;
            var driverIds = earnings.Select(e => e.DriverId).ToList();

            var existing = await _context.DriverEarnings
                .Where(e => e.Date == date && driverIds.Contains(e.DriverId))
                .ToListAsync();
            var existingByDriverId = existing.ToDictionary(e => e.DriverId);

            var toInsert = new List<DriverEarnings>();

            foreach (var item in earnings)
            {
                if (existingByDriverId.TryGetValue(item.DriverId, out var existingRecord))
                {
                    existingRecord.TotalEarnings = item.TotalEarnings;
                    existingRecord.DeliveryCount = item.DeliveryCount;
                }
                else
                {
                    toInsert.Add(item);
                }
            }

            if (toInsert.Count > 0)
            {
                await _context.DriverEarnings.AddRangeAsync(toInsert);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<DriverEarnings>> GetByDriverIdAsync(string driverId, DateTime fromDate, DateTime toDate)
        {
            return await _context.DriverEarnings
                .Where(e => e.DriverId == driverId && e.Date >= fromDate && e.Date <= toDate)
                .OrderBy(e => e.Date)
                .ToListAsync();
        }
    }
}
