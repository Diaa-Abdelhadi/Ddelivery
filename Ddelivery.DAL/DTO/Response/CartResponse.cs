using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.DTO.Response
{
    public class CartResponse
    {
        public int MealId { get; set; }
        public string MealName { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal PriceTotal => Count * Price;
    }
}
