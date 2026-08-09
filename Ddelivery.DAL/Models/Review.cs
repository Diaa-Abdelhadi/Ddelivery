using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Models
{

    public class Review
    {
        public int Id { get; set; }
        public int MealId { get; set; }
        public Meal Meal { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
