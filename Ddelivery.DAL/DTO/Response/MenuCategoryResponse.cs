using Ddelivery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ddelivery.DAL.DTO.Response
{
    public class MenuCategoryResponse
    {
        public int Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status status { get; set; }
        public int RestaurantId { get; set; }
        public List<MenuCategoryTranslationResponse> Translations { get; set; }
    }
}
