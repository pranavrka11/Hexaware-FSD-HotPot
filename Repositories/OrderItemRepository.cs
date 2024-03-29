﻿using HotPot.Context;
using HotPot.Exceptions;
using HotPot.Interfaces;
using HotPot.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPot.Repositories
{
    public class OrderItemRepository : IRepository<(int OrderId, int MenuId), string, OrderItem>
    {
        private readonly HotpotContext _context;
        private readonly ILogger<OrderItemRepository> _logger;
        public OrderItemRepository(HotpotContext context, ILogger<OrderItemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new order item to the database.
        /// </summary>
        /// <param name="item">The order item to add.</param>
        /// <returns>The added order item.</returns>
        public async Task<OrderItem> Add(OrderItem item)
        {
            _context.Add(item);
            _context.SaveChanges();

            LogInformation($"Order Item Added: OrderId={item.OrderId}, MenuId={item.MenuId}");

            return item;
        }

        /// <summary>
        /// Deletes an existing order item from the database.
        /// </summary>
        /// <param name="key">The composite key of the order item to delete (OrderId, MenuId).</param>
        /// <returns>The deleted order item, if found.</returns>
        public async Task<OrderItem> Delete((int OrderId, int MenuId) key)
        {
            var orderItem = await GetAsync(key);

            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
                _context.SaveChanges();

                LogInformation($"Order Item Deleted: OrderId={orderItem.OrderId}, MenuId={orderItem.MenuId}");
                return orderItem;
            }

            throw new NoOrderItemFoundException();
        }

        /// <summary>
        /// Retrieves an order item by its composite key (OrderId, MenuId).
        /// </summary>
        /// <param name="key">The composite key of the order item to retrieve.</param>
        /// <returns>The retrieved order item, if found.</returns>
        public async Task<OrderItem> GetAsync((int OrderId, int MenuId) key)
        {
            var orderItem = _context.OrderItems
                .Where(oi => oi.OrderId == key.OrderId && oi.MenuId == key.MenuId)
                .FirstOrDefault();

            if (orderItem != null)
            {
                return orderItem;
            }

            throw new NoOrderItemFoundException();
        }

        /// <summary>
        /// Retrieves all order items from the database, including their associated order and menu information.
        /// </summary>
        /// <returns>A list of all order items with their related data.</returns>

        public async Task<List<OrderItem>> GetAsync()
        {
            var orderItems = await _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Menu)
                .ToListAsync();


            return orderItems;
        }

        /// <summary>
        /// Updates an existing order item in the database.
        /// </summary>
        /// <param name="item">The order item to update.</param>
        /// <returns>The updated order item.</returns>
        public async Task<OrderItem> Update(OrderItem item)
        {
            var orderItem = await GetAsync((item.OrderId, item.MenuId));

            if (orderItem != null)
            {
                _context.Entry<OrderItem>(item).State = EntityState.Modified;
                 _context.SaveChanges();

                LogInformation($"Order Item Updated: OrderId={item.OrderId}, MenuId={item.MenuId}");
                return orderItem;
            }
            throw new NoOrderItemFoundException();
            
        }


        /// <summary>
        /// Logs an informational message using the provided logger.
        /// </summary>
        /// <param name="message">The message to log.</param>

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public Task<OrderItem> GetAsync(string key)
        {
            throw new NotImplementedException();
        }
    }
}
