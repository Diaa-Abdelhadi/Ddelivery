using Ddelivery.DAL.DTO.Response;
using Ddelivery.DAL.Models;
using Ddelivery.DAL.Repository;
using Mapster;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.BLL.Service
{
    public class EarningsService : IEarningsService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRestaurantEarningsRepository _restaurantEarningsRepository;
        private readonly IDriverEarningsRepository _driverEarningsRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly decimal _driverFeePerDelivery;

        public EarningsService(
            IOrderRepository orderRepository,
            IRestaurantEarningsRepository restaurantEarningsRepository,
            IDriverEarningsRepository driverEarningsRepository,
            IRestaurantRepository restaurantRepository,
            IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _restaurantEarningsRepository = restaurantEarningsRepository;
            _driverEarningsRepository = driverEarningsRepository;
            _restaurantRepository = restaurantRepository;
            _driverFeePerDelivery = configuration.GetValue<decimal>("Earnings:DriverFeePerDelivery", 5.0m);
        }

        public async Task<int> CalculateDailyEarningsAsync(DateTime date)
        {
            var deliveredOrders = await _orderRepository.GetDeliveredOrdersByDateAsync(date);

            var restaurantEarnings = deliveredOrders
                .GroupBy(o => o.RestaurantId)
                .Select(group => new RestaurantEarnings
                {
                    RestaurantId = group.Key,
                    Date = date.Date,
                    TotalRevenue = group.Sum(o => o.OrderItems.Sum(oi => oi.TotalPrice)),
                    OrderCount = group.Count()
                })
                .ToList();

            var driverEarnings = deliveredOrders
                .Where(o => o.DriverId != null)
                .GroupBy(o => o.DriverId)
                .Select(group => new DriverEarnings
                {
                    DriverId = group.Key!,
                    Date = date.Date,
                    TotalEarnings = group.Count() * _driverFeePerDelivery,
                    DeliveryCount = group.Count()
                })
                .ToList();

            await _restaurantEarningsRepository.SaveAllAsync(restaurantEarnings);
            await _driverEarningsRepository.SaveAllAsync(driverEarnings);

            return restaurantEarnings.Count + driverEarnings.Count;
        }

        public async Task<List<RestaurantEarningsResponse>?> GetRestaurantEarningsAsync(int restaurantId, string ownerId, DateTime fromDate, DateTime toDate)
        {
            var restaurant = await _restaurantRepository.FindByIdAsync(restaurantId);
            if (restaurant is null || restaurant.OwnerId != ownerId)
            {
                return null;
            }

            var earnings = await _restaurantEarningsRepository.GetByRestaurantIdAsync(restaurantId, fromDate.Date, toDate.Date);
            return earnings.Adapt<List<RestaurantEarningsResponse>>();
        }

        public async Task<List<DriverEarningsResponse>> GetDriverEarningsAsync(string driverId, DateTime fromDate, DateTime toDate)
        {
            var earnings = await _driverEarningsRepository.GetByDriverIdAsync(driverId, fromDate.Date, toDate.Date);
            return earnings.Adapt<List<DriverEarningsResponse>>();
        }
    }
}
