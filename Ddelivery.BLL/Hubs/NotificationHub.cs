using Ddelivery.DAL.Models;
using Ddelivery.DAL.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ddelivery.BLL.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IOrderRepository _orderRepository;

        public NotificationHub(IRestaurantRepository restaurantRepository, IOrderRepository orderRepository)
        {
            _restaurantRepository = restaurantRepository;
            _orderRepository = orderRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
            }
            await base.OnConnectedAsync();
        }

        public async Task JoinRestaurantGroup(int restaurantId)
        {
            var ownerId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var restaurant = await _restaurantRepository.FindByIdAsync(restaurantId);
            if (restaurant is null || restaurant.OwnerId != ownerId)
            {
                throw new HubException("Not authorized for this restaurant");
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, $"restaurant-{restaurantId}");
        }

        public async Task SendDriverLocation(int orderId, double latitude, double longitude)
        {
            var driverId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _orderRepository.FindByIdAsync(orderId);
            if (order is null || order.DriverId != driverId || order.OrderStatus != OrderStatus.OnTheWay)
            {
                throw new HubException("Not authorized to update location for this order");
            }

            await Clients.Group($"user-{order.UserId}").SendAsync("DriverLocationUpdated", new
            {
                orderId,
                latitude,
                longitude
            });
        }
    }
}
