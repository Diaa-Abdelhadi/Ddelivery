using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Models
{
    [PrimaryKey(nameof(MealId), nameof(UserId))]
    public class Cart
    {
        public int MealId { get; set; }
        public Meal Meal { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int count { get; set; }
    }
}
