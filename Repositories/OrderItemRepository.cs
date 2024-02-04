using HotPot.Context;
using HotPot.Interfaces;
using HotPot.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPot.Repositories
{
    public class OrderItemRepository : IRepository<(int OrderId, int MenuId), OrderItem>
    {
        private readonly HotpotContext _context;
        private readonly ILogger<OrderItemRepository> _logger;
        public OrderItemRepository(HotpotContext context, ILogger<OrderItemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        

        public async Task<OrderItem> Add(OrderItem item)
        {
            _context.OrdersItems.Add(item);
            await _context.SaveChangesAsync();

            LogInformation($"Order Item Added: OrderId={item.OrderId}, MenuId={item.MenuId}");

            return item;
        }
         public async Task<OrderItem> Delete((int OrderId, int MenuId) key)
        {
            var orderItem = await GetAsync(key);

            if (orderItem != null)
            {
                _context.OrdersItems.Remove(orderItem);
                await _context.SaveChangesAsync();

                LogInformation($"Order Item Deleted: OrderId={orderItem.OrderId}, MenuId={orderItem.MenuId}");
            }

            return orderItem;
        }
        public async Task<OrderItem> GetAsync((int OrderId, int MenuId) key)
        {
            var orderItem = await _context.OrdersItems
                .Where(oi => oi.OrderId == key.OrderId && oi.MenuId == key.MenuId)
                .FirstOrDefaultAsync();

            if (orderItem != null)
            {
                return orderItem;
            }

            throw new NoOrderItemFoundException();
        }

        public async Task<List<OrderItem>> GetAsync()
        {
            var orderItems = await _context.OrdersItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Menu)
                .ToListAsync();

            return orderItems;
        }
        public async Task<OrderItem> Update(OrderItem item)
        {
            var orderItem = await GetAsync((item.OrderId, item.MenuId));

            if (orderItem != null)
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                LogInformation($"Order Item Updated: OrderId={item.OrderId}, MenuId={item.MenuId}");
            }

            return orderItem;
        }

       

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

    }
}
