using Ddelivery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ddelivery.DAL.DTO.Response
{
    public class MealResponse
    {
        public int Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status status { get; set; }
        public int RestaurantId { get; set; }
        public int MenuCategoryId { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public string MainImage { get; set; }
        public List<string> SubImages { get; set; }
        public List<MealTranslationResponse> Translations { get; set; }
    }
}
