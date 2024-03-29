﻿using HotPot.Context;
using HotPot.Exceptions;
using HotPot.Interfaces;
using HotPot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotPot.Repositories
{
    public class MenuRepository : IRepository<int, string, Menu>
    {
        readonly HotpotContext _context;
        readonly ILogger<MenuRepository> _logger;
        public MenuRepository(HotpotContext context, ILogger<MenuRepository> logger) 
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new menu item to the database.
        /// </summary>
        /// <param name="item">The menu item to add.</param>
        /// <returns>The added menu item.</returns>
        public async Task<Menu> Add(Menu item)
        {
            _context.Add(item);
            _context.SaveChanges();
            LogInformation($"Menu Added: {item.MenuId}");
            return item;
        }

        /// <summary>
        /// Deletes an existing menu item from the database.
        /// </summary>
        /// <param name="key">The ID of the menu item to delete.</param>
        /// <returns>The deleted menu item, if found.</returns>
        public async Task<Menu> Delete(int key)
        {
            var menu = await GetAsync(key);
            if (menu != null)
            {
                _context.Remove(menu);
                _context.SaveChanges();
                LogInformation($"Menu deleted: {menu.MenuId}");
                return menu;
            }
            return null;
        }

        /// <summary>
        /// Retrieves a menu item by its ID.
        /// </summary>
        /// <param name="key">The ID of the menu item to retrieve.</param>
        /// <returns>The retrieved menu item, if found.</returns>
        public async Task<Menu> GetAsync(int key)
        {
            var menus = await GetAsync();
            var menu = menus.FirstOrDefault(m => m.MenuId == key);
            if (menu != null)
            {
                return menu;
            }
            throw new NoMenuFoundException();
        }

        /// <summary>
        /// Retrieves all menu items from the database, including their associated nutritional information and restaurant.
        /// </summary>
        /// <returns>A list of all menu items with their related data.</returns>
        public async Task<List<Menu>> GetAsync()
        {
            var menus = await _context.Menus
                 .Include(m => m.NutritionalInfo)
                 .Include(m => m.Restaurant)
                 .ToListAsync();
            LogInformation("Menu items retrieved successfully.");
            return menus;
        }

        /// <summary>
        /// Updates an existing menu item in the database.
        /// </summary>
        /// <param name="item">The menu item to update.</param>
        /// <returns>The updated menu item.</returns>
        public async Task<Menu> Update(Menu item)
        {
            var menu = await GetAsync(item.MenuId);
            if (menu != null)
            {
                _context.Entry<Menu>(item).State = EntityState.Modified;
                _context.SaveChanges();
                LogInformation($"Menu Updated: {item.MenuId}");
            }
            return menu;
        }

        /// <summary>
        /// Retrieves a menu item by its name.
        /// </summary>
        /// <param name="name">The name of the menu item to retrieve.</param>
        /// <returns>The retrieved menu item, if found.</returns>
        public async Task<Menu> GetAsync(string name)
        {
            var menus = _context.Menus.ToList();
            var menu = menus.FirstOrDefault(m =>m.Name == name);
            if(menu != null)
            {
                return menu;
            }
            throw new NoMenuFoundException("No Menu Found");
        }

        /// <summary>
        /// Logs an informational message using the provided logger.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
