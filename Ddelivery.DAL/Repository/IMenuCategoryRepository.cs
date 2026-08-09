using Ddelivery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Repository
{
    public interface IMenuCategoryRepository
    {
        Task<List<MenuCategory>> GetAllAsync();
        Task<MenuCategory> CreateAsync(MenuCategory request);
        Task<MenuCategory?> UpdateAsync(MenuCategory request);
        Task<MenuCategory?> FindByIdAsync(int id);
        Task DeleteAsync(MenuCategory category);
        Task<List<MenuCategory>> GetByRestaurantIdAsync(int restaurantId);
    }
}
