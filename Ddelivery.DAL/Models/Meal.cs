using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Models
{
    public class Meal : BaseModel
    {
        public decimal Discount { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public string MainImage { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public int MenuCategoryId { get; set; }
        public MenuCategory MenuCategory { get; set; }
        public List<MealTranslation> Translations { get; set; }
        public List<MealImage> SubImages { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
