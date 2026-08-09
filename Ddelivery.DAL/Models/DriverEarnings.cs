using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Models
{
    public class DriverEarnings
    {
        public int Id { get; set; }
        public string DriverId { get; set; }
        public ApplicationUser Driver { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalEarnings { get; set; }
        public int DeliveryCount { get; set; }
    }
}
