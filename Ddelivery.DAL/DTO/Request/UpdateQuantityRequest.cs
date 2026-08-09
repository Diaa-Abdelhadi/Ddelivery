using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.DTO.Request
{
    public class UpdateQuantityRequest
    {
        [Range(0, int.MaxValue)]
        public int Count { get; set; }
    }
}
