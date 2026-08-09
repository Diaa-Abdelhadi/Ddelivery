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
    public class MealRepository : IMealRepository
    {
        private readonly ApplicationDbContext _context;

        public MealRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Meal> AddAsync(Meal request)
        {
            await _context.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;

        }
        public async Task<Meal?> FindByIdAsync(int id)
        {
            var response = await _context.Meals.Include(c => c.Translations).Include(c => c.SubImages)
                .Include(c => c.Reviews).ThenInclude(r => r.User)
                .FirstOrDefaultAsync(c => c.Id == id);
            return response;
        }

        public async Task<bool> DecreaseQuantitesAsync(List<(int mealId, int quantity)> meals)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            foreach (var (mealId, quantity) in meals)
            {
                var affected = await _context.Meals
                    .Where(m => m.Id == mealId && m.Quantity >= quantity)
                    .ExecuteUpdateAsync(s => s.SetProperty(m => m.Quantity, m => m.Quantity - quantity));

                if (affected == 0)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }

            await transaction.CommitAsync();
            return true;
        }
        public IQueryable<Meal> Query()
        {
            return _context.Meals.Include(p => p.Translations).AsNoTracking().AsQueryable();


        }
        public async Task<Meal?> UpdateAsync(Meal meal)
        {
            _context.Meals.Update(meal);
            await _context.SaveChangesAsync();
            return meal;
        }
        public async Task IncreaseQuantitiesAsync(List<(int mealId, int quantity)> meals)
        {
            foreach (var (mealId, quantity) in meals)
            {
                await _context.Meals
                    .Where(m => m.Id == mealId)
                    .ExecuteUpdateAsync(s => s.SetProperty(m => m.Quantity, m => m.Quantity + quantity));
            }
        }
    }
}
