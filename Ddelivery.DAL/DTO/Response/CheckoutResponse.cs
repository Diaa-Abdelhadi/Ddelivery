using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.DTO.Response
{
    public class CheckoutResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<int> OrderIds { get; set; }
    }
}
