using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Models
{
    [PrimaryKey(nameof(MealId), nameof(OrderId))]
    public class OrderItem
    {
        public int MealId { get; set; }
        public Meal Meal { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
