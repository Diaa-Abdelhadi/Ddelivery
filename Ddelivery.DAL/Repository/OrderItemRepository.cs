using Ddelivery.DAL.Data;
using Ddelivery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Repository
{

    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<OrderItem>> CreateRangeAsync(List<OrderItem> request)
        {
            await _context.OrderItems.AddRangeAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }
    }
}
