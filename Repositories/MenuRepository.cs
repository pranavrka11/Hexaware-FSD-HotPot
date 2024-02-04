using HotPot.Context;
using HotPot.Interfaces;
using HotPot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotPot.Repositories
{
    public class MenuRepository : IRepository<int, Menu>
    {
        private readonly HotpotContext _context;
        private readonly ILogger<MenuRepository> _logger;
        public MenuRepository(HotpotContext context, ILogger<MenuRepository> logger) 
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Menu> Add(Menu item)
        {
            _context.Menus.Add(item);
            await _context.SaveChangesAsync();
            LogInformation($"Menu Added: {item.MenuId}");
            return item;
        }
        public async Task<Menu> Delete(int key)
        {
            var menu = await GetAsync(key);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
                await _context.SaveChangesAsync();
                LogInformation($"Menu deleted: {menu.MenuId}");
                return menu;
            }
            return null;
        }


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

        public async Task<List<Menu>> GetAsync()
        {
            var menus = await _context.Menus
                .Include(m => m.NutritionalInfo)
                .Include(m => m.Restaurant)
                .ToListAsync();
            LogInformation("Menu items retrieved successfully.");
            return menus;
        }

        public async Task<Menu> Update(Menu item)
        {
            var menu = await GetAsync(item.MenuId);
            if (menu != null)
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                LogInformation($"Menu Updated: {item.MenuId}");
            }
            return menu;
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
