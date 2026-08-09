using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Models
{
    public class Restaurant : BaseModel
    {
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string PhoneNumber { get; set; }
        public double Rate { get; set; }
        public string MainImage { get; set; }
        public List<RestaurantTranslation> Translations { get; set; }
        public List<MenuCategory> MenuCategories { get; set; }
        public List<Meal> Meals { get; set; }
    }
}
