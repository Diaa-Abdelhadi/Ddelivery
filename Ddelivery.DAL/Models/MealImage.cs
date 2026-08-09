using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Models
{
    public class MealImage
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public int MealId { get; set; }
        public Meal Meal { get; set; }
    }
}
