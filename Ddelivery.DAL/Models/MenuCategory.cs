using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Models
{
    public class MenuCategory : BaseModel
    {
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public List<MenuCategoryTranslation> Translations { get; set; }
        public List<Meal> Meals { get; set; }
    }
}
