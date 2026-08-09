using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.DTO.Request
{
    public class MealRequest
    {
        [Required, MinLength(1)]
        public List<MealTranslationRequest> Translations { get; set; }

        [Range(0.01, (double)decimal.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, (double)decimal.MaxValue)]
        public decimal Discount { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(1, int.MaxValue)]
        public int MenuCategoryId { get; set; }

        [Required]
        public IFormFile MainImage { get; set; }

        [Required, MinLength(1)]
        public List<IFormFile> SubImages { get; set; }
    }
}
