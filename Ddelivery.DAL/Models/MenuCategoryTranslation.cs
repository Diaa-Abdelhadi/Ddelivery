using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Models
{
    public class MenuCategoryTranslation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Language { get; set; } = "en";
        public int MenuCategoryId { get; set; }
        public MenuCategory MenuCategory { get; set; }
    }
}
