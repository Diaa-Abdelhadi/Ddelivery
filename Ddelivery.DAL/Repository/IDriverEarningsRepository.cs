using Ddelivery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Repository
{
    public interface IDriverEarningsRepository
    {
        Task SaveAllAsync(List<DriverEarnings> earnings);
        Task<List<DriverEarnings>> GetByDriverIdAsync(string driverId, DateTime fromDate, DateTime toDate);
    }
}
