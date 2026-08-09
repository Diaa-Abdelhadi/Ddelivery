using Ddelivery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ddelivery.DAL.DTO.Response
{

    public class OrderResponse
    {
        public int Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatus OrderStatus { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime? DeliveredAt { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethodEnum PaymentMethod { get; set; }
        public decimal? AmountPaid { get; set; }
        public int RestaurantId { get; set; }
        public string DeliveryAddress { get; set; }
        public List<OrderItemResponse> Items { get; set; }
        public string? DriverId { get; set; }

    }
}
