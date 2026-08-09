using Ddelivery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Repository
{
    public interface IOrderItemRepository
    {
        Task<List<OrderItem>> CreateRangeAsync(List<OrderItem> request);
    }
}
