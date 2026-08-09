using Ddelivery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Repository
{
    public interface ICartRepository
    {
        Task<Cart> Createasync(Cart request);
        Task<List<Cart>> GetUserCartAsync(string userId);
        Task<Cart?> GetCartItemAsync(string userId, int mealId);
        Task<Cart> UpdateAsync(Cart cart);
        Task ClearCartAsync(string userId);
        Task DeleteAsync(Cart cart);
    }
}
