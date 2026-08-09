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
    public class MenuCategoryRepository : IMenuCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MenuCategory> CreateAsync(MenuCategory request)
        {
            await _context.MenuCategories.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<List<MenuCategory>> GetAllAsync()
        {
            return await _context.MenuCategories.Include(c => c.Translations).Include(c => c.user).ToListAsync();
        }
        public async Task<MenuCategory> FindByIdAsync(int id)
        {
            return await _context.MenuCategories.Include(c => c.Translations).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task DeleteAsync(MenuCategory category)
        {
            _context.MenuCategories.Remove(category);
            await _context.SaveChangesAsync();
        }
        public async Task<MenuCategory?> UpdateAsync(MenuCategory request)
        {
            _context.MenuCategories.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }
        public async Task<List<MenuCategory>> GetByRestaurantIdAsync(int restaurantId)
        {
            return await _context.MenuCategories
                .Include(c => c.Translations)
                .Where(c => c.RestaurantId == restaurantId)
                .ToListAsync();
        }
    }
}
