using Ddelivery.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.DTO.Request
{
    public class CheckoutRequest
    {
        [Required]
        public string DeliveryAddress { get; set; }

        [Range(-90, 90)]
        public double DeliveryLatitude { get; set; }

        [Range(-180, 180)]
        public double DeliveryLongitude { get; set; }

        [EnumDataType(typeof(PaymentMethodEnum))]
        public PaymentMethodEnum PaymentMethod { get; set; }
    }
}
