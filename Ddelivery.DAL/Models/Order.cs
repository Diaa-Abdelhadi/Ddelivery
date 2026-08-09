using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Models
{
    public enum OrderStatus
    {
        Pending = 1,
        Accepted = 2,
        Preparing = 3,
        OnTheWay = 4,
        Delivered = 5,
        Cancelled = 6
    }
    public enum PaymentMethodEnum
    {
        Cash = 1, Card = 2
    }
    public class Order
    {
        private static readonly Dictionary<OrderStatus, OrderStatus[]> AllowedTransitions = new()
        {
            [OrderStatus.Pending] = new[] { OrderStatus.Accepted, OrderStatus.Cancelled },
            [OrderStatus.Accepted] = new[] { OrderStatus.Preparing, OrderStatus.Cancelled },
            [OrderStatus.Preparing] = new[] { OrderStatus.OnTheWay },
            [OrderStatus.OnTheWay] = new[] { OrderStatus.Delivered }
        };

        public bool CanTransitionTo(OrderStatus newStatus) =>
            AllowedTransitions.TryGetValue(OrderStatus, out var allowed) && allowed.Contains(newStatus);

        public int Id { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public DateTime OrderTime { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveredAt { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentId { get; set; }
        public decimal? AmountPaid { get; set; }
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public string? DriverId { get; set; }
        public ApplicationUser Driver { get; set; }
        public string DeliveryAddress { get; set; }
        public double DeliveryLatitude { get; set; }
        public double DeliveryLongitude { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
