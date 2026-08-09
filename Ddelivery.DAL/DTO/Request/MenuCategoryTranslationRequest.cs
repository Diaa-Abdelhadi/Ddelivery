using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.DTO.Request
{
    public class MenuCategoryTranslationRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Language { get; set; }
    }
}
