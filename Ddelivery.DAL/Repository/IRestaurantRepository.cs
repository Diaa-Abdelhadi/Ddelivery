using Ddelivery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Repository
{
    public interface IRestaurantRepository
    {
        IQueryable<Restaurant> Query();
        Task<Restaurant> CreateAsync(Restaurant request);
        Task<Restaurant?> UpdateAsync(Restaurant request);
        Task<Restaurant?> FindByIdAsync(int id);
        Task DeleteAsync(Restaurant restaurant);
    }

}
