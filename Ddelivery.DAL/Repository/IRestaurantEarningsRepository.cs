using Ddelivery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Repository
{
    public interface IRestaurantEarningsRepository
    {
        Task SaveAllAsync(List<RestaurantEarnings> earnings);
        Task<List<RestaurantEarnings>> GetByRestaurantIdAsync(int restaurantId, DateTime fromDate, DateTime toDate);
    }
}
