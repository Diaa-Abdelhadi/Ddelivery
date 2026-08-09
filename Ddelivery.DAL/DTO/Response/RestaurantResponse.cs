using Ddelivery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ddelivery.DAL.DTO.Response
{
    public class RestaurantResponse
    {
        public int Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status status { get; set; }
        public string CreatedBy { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string PhoneNumber { get; set; }
        public double Rate { get; set; }
        public string MainImage { get; set; }
        public List<RestaurantTranslationResponse> Translations { get; set; }
    }
}
