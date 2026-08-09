using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.DTO.Response
{
    public class DriverEarningsResponse
    {
        public DateTime Date { get; set; }
        public decimal TotalEarnings { get; set; }
        public int DeliveryCount { get; set; }
    }
}
