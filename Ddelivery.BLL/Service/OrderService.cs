using Ddelivery.DAL.DTO.Request;
using Ddelivery.DAL.DTO.Response;
using Ddelivery.DAL.Models;
using Ddelivery.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ddelivery.BLL.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Ddelivery.BLL.Service
{
    public class OrderService : IOrderService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMealRepository _mealRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public OrderService(
            ICartRepository cartRepository,
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IMealRepository mealRepository,
            IRestaurantRepository restaurantRepository,
            IHubContext<NotificationHub> hubContext)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _mealRepository = mealRepository;
            _restaurantRepository = restaurantRepository;
            _hubContext = hubContext;
        }

        public async Task<CheckoutResponse> CheckoutAsync(string userId, CheckoutRequest request)
        {
            var cartItems = await _cartRepository.GetUserCartAsync(userId);
            if (cartItems is null || !cartItems.Any())
            {
                return new CheckoutResponse { Success = false, Message = "Cart is empty" };
            }

            foreach (var item in cartItems)
            {
                if (item.Meal.Quantity < item.count)
                {
                    return new CheckoutResponse { Success = false, Message = "Not enough stock for one or more items in your cart" };
                }
            }

            var decreaseList = cartItems.Select(c => (mealId: c.MealId, quantity: c.count)).ToList();
            var stockReserved = await _mealRepository.DecreaseQuantitesAsync(decreaseList);
            if (!stockReserved)
            {
                return new CheckoutResponse { Success = false, Message = "Not enough stock for one or more items in your cart" };
            }

            var orderIds = new List<int>();
            var restaurantGroups = cartItems.GroupBy(c => c.Meal.RestaurantId);

            foreach (var group in restaurantGroups)
            {
                var order = new Order
                {
                    UserId = userId,
                    RestaurantId = group.Key,
                    PaymentMethod = request.PaymentMethod,
                    DeliveryAddress = request.DeliveryAddress,
                    DeliveryLatitude = request.DeliveryLatitude,
                    DeliveryLongitude = request.DeliveryLongitude
                };
                await _orderRepository.CreateAsync(order);

                var orderItems = group.Select(c => new OrderItem
                {
                    MealId = c.MealId,
                    OrderId = order.Id,
                    Quantity = c.count,
                    UnitPrice = c.Meal.Price,
                    TotalPrice = c.Meal.Price * c.count
                }).ToList();

                await _orderItemRepository.CreateRangeAsync(orderItems);
                orderIds.Add(order.Id);
                await _hubContext.Clients.Group($"restaurant-{order.RestaurantId}").SendAsync("NewOrder", new
                {
                    orderId = order.Id,
                    restaurantId = order.RestaurantId
                });
            }

            await _cartRepository.ClearCartAsync(userId);

            return new CheckoutResponse { Success = true, Message = "Order placed successfully", OrderIds = orderIds };
        }

        public async Task<List<OrderResponse>> GetMyOrdersAsync(string userId, string lang = "en")
        {
            var orders = await _orderRepository.GetByUserIdAsync(userId);
            return orders.BuildAdapter().AddParameters("lang", lang).AdaptToType<List<OrderResponse>>();
        }

        public async Task<OrderResponse?> GetOrderDetailsAsync(string userId, int orderId, string lang = "en")
        {
            var order = await _orderRepository.FindByIdAsync(orderId);
            if (order is null || order.UserId != userId)
            {
                return null;
            }
            return order.BuildAdapter().AddParameters("lang", lang).AdaptToType<OrderResponse>();
        }

        public async Task<PagintedResponse<OrderResponse>?> GetOrdersForOwnerAsync(int restaurantId, string ownerId, string lang = "en", int page = 1, int limit = 10)
        {
            var restaurant = await _restaurantRepository.FindByIdAsync(restaurantId);
            if (restaurant is null || restaurant.OwnerId != ownerId)
            {
                return null;
            }

            var (orders, totalCount) = await _orderRepository.GetByRestaurantIdAsync(restaurantId, page, limit);
            var response = orders.BuildAdapter().AddParameters("lang", lang).AdaptToType<List<OrderResponse>>();

            return new PagintedResponse<OrderResponse>
            {
                TotalCount = totalCount,
                Page = page,
                Limit = limit,
                Data = response
            };
        }

        public async Task<BaseResponse> UpdateOrderStatusForOwnerAsync(int orderId, OrderStatus newStatus, string ownerId)
        {
            if (newStatus != OrderStatus.Accepted && newStatus != OrderStatus.Preparing && newStatus != OrderStatus.Cancelled)
            {
                return new BaseResponse { Success = false, Message = "Restaurant owners can only accept, prepare, or cancel orders" };
            }

            var order = await _orderRepository.FindByIdAsync(orderId);
            if (order is null)
            {
                return new BaseResponse { Success = false, Message = "Order not found" };
            }

            var restaurant = await _restaurantRepository.FindByIdAsync(order.RestaurantId);
            if (restaurant is null || restaurant.OwnerId != ownerId)
            {
                return new BaseResponse { Success = false, Message = "You are not authorized to manage this order" };
            }

            return await TransitionOrderStatusAsync(order, newStatus);
        }

        public async Task<List<OrderResponse>> GetAvailableOrdersForDriverAsync(string lang = "en")
        {
            var orders = await _orderRepository.GetAvailableForDriverAsync();
            return orders.BuildAdapter().AddParameters("lang", lang).AdaptToType<List<OrderResponse>>();
        }

        public async Task<BaseResponse> UpdateOrderStatusForDriverAsync(int orderId, OrderStatus newStatus, string driverId)
        {
            if (newStatus != OrderStatus.OnTheWay && newStatus != OrderStatus.Delivered)
            {
                return new BaseResponse { Success = false, Message = "Drivers can only pick up or deliver orders" };
            }

            var order = await _orderRepository.FindByIdAsync(orderId);
            if (order is null)
            {
                return new BaseResponse { Success = false, Message = "Order not found" };
            }

            if (newStatus == OrderStatus.OnTheWay)
            {
                if (order.DriverId != null)
                {
                    return new BaseResponse { Success = false, Message = "Order already claimed by another driver" };
                }
                order.DriverId = driverId;
            }
            else if (order.DriverId != driverId)
            {
                return new BaseResponse { Success = false, Message = "You are not assigned to this order" };
            }

            return await TransitionOrderStatusAsync(order, newStatus);
        }

        public async Task<List<OrderResponse>> GetMyDeliveriesAsync(string driverId, string lang = "en")
        {
            var orders = await _orderRepository.GetByDriverIdAsync(driverId);
            return orders.BuildAdapter().AddParameters("lang", lang).AdaptToType<List<OrderResponse>>();
        }

        private async Task<BaseResponse> TransitionOrderStatusAsync(Order order, OrderStatus newStatus)
        {
            if (!order.CanTransitionTo(newStatus))
            {
                return new BaseResponse { Success = false, Message = $"Cannot move order from {order.OrderStatus} to {newStatus}" };
            }

            order.OrderStatus = newStatus;
            if (newStatus == OrderStatus.Delivered)
            {
                order.DeliveredAt = DateTime.UtcNow;
            }
            await _orderRepository.UpdateAsync(order);
            await _hubContext.Clients.Group($"user-{order.UserId}").SendAsync("OrderStatusChanged", new
            {
                orderId = order.Id,
                status = order.OrderStatus.ToString()
            });
            return new BaseResponse { Success = true, Message = "Order status updated successfully" };
        }
        public async Task<int> CancelStaleOrdersAsync(int abandonThresholdMinutes)
        {
            var cutoff = DateTime.UtcNow.AddMinutes(-abandonThresholdMinutes);
            var staleOrders = await _orderRepository.GetStalePendingOrdersAsync(cutoff);

            if (staleOrders.Count == 0)
            {
                return 0;
            }

            var restockList = new List<(int mealId, int quantity)>();

            foreach (var order in staleOrders)
            {
                order.OrderStatus = OrderStatus.Cancelled;
                await _orderRepository.UpdateAsync(order);

                foreach (var item in order.OrderItems)
                {
                    restockList.Add((item.MealId, item.Quantity));
                }
            }

            await _mealRepository.IncreaseQuantitiesAsync(restockList);

            return staleOrders.Count;
        }
    }
}
