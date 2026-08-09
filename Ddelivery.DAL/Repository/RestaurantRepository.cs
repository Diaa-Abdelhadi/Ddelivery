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
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly ApplicationDbContext _context;

        public RestaurantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Restaurant> CreateAsync(Restaurant request)
        {
            await _context.Restaurants.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public IQueryable<Restaurant> Query()
        {
            return _context.Restaurants.Include(r => r.Translations).AsNoTracking().AsQueryable();
        }

        public async Task<Restaurant> FindByIdAsync(int id)
        {
            return await _context.Restaurants.Include(r => r.Translations).Include(r => r.Owner).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task DeleteAsync(Restaurant restaurant)
        {
            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
        }
        public async Task<Restaurant?> UpdateAsync(Restaurant request)
        {
            _context.Restaurants.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }
    }
}
