using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Models
{
    public class MealTranslation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Language { get; set; } = "en";
        public int MealId { get; set; }
        public Meal Meal { get; set; }
    }
}
