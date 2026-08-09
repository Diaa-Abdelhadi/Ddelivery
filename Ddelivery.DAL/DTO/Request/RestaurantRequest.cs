using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.DTO.Request
{
    public class RestaurantRequest
    {
        [Required, MinLength(1)]
        public List<RestaurantTranslationRequest> Translations { get; set; }

        [Required]
        public string Address { get; set; }

        [Range(-90, 90)]
        public double Latitude { get; set; }

        [Range(-180, 180)]
        public double Longitude { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public IFormFile MainImage { get; set; }
    }
}
