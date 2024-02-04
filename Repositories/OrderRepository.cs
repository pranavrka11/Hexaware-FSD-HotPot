using HotPot.Context;
using HotPot.Interfaces;
using HotPot.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPot.Repositories
{
    public class OrderRepository : IRepository<int, Order>
    {
        private readonly HotpotContext _context;
        private readonly ILogger<OrderRepository> _logger;
        public OrderRepository(HotpotContext context, ILogger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Order> Add(Order item)
        {
            _context.Orders.Add(item);
            await _context.SaveChangesAsync();

            LogInformation($"Order Added: {item.OrderId}");

            return item;
        }
        public async Task<Order> Delete(int key)
        {
            var order = await GetAsync(key);

            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                LogInformation($"Order Deleted: {order.OrderId}");
            }

            return order;
        }

        public async Task<Order> GetAsync(int key)
        {
            var orders = await GetAsync();
            var order = orders.FirstOrDefault(o => o.OrderId == key);

            if (order != null)
            {
                return order;
            }

            throw new NoOrderFoundException();
        }

        public async Task<List<Order>> GetAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Restaurant)
                .Include(o => o.DeliveryPartner)
                .Include(o => o.OrderItems)
                .ToListAsync();

            return orders;
        }
        public async Task<Order> Update(Order item)
        {
            var order = await GetAsync(item.OrderId);

            if (order != null)
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                LogInformation($"Order Updated: {item.OrderId}");
            }

            return order;
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
