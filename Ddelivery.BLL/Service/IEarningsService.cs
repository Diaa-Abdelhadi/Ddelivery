using Ddelivery.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.BLL.Service
{

    public interface IEarningsService
    {
        Task<int> CalculateDailyEarningsAsync(DateTime date);
        Task<List<RestaurantEarningsResponse>?> GetRestaurantEarningsAsync(int restaurantId, string ownerId, DateTime fromDate, DateTime toDate);
        Task<List<DriverEarningsResponse>> GetDriverEarningsAsync(string driverId, DateTime fromDate, DateTime toDate);
    }
}
