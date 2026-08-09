using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.DTO.Request
{
    public class AddToCartRequest
    {
        [Range(1, int.MaxValue)]
        public int MealId { get; set; }

        [Range(1, int.MaxValue)]
        public int count { get; set; } = 1;
    }
}
