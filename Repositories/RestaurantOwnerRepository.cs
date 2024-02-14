using HotPot.Context;
using HotPot.Exceptions;
using HotPot.Interfaces;
using HotPot.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPot.Repositories
{
    public class RestaurantOwnerRepository : IRepository<int, string, RestaurantOwner>
    {
        private readonly HotpotContext _context;
        private readonly ILogger<RestaurantOwnerRepository> _logger;

        public RestaurantOwnerRepository(HotpotContext context, ILogger<RestaurantOwnerRepository> logger)
        {
            _context = context;
            _logger = logger;  
        }

        public async Task<RestaurantOwner> Add(RestaurantOwner item)
        {
            _context.Add(item);
            _context.SaveChanges();
            LogInformation($"Restaurant Owner Added: {item.OwnerId}");
            return item;
        }

        public async Task<RestaurantOwner> Delete(int key)
        {
            var owner = await GetAsync(key);
            if (owner != null)
            {
                _context.RestaurantOwners.Remove(owner);
                _context.SaveChanges();
                LogInformation($"Restaurant Owner Deleted: {owner.OwnerId}");
                return owner;
            }
            throw new Exception();
        }

        public async Task<RestaurantOwner> GetAsync(int key)
        {
            var owners = await GetAsync();
            var owner = owners.FirstOrDefault(o=> o.OwnerId == key);
            if (owner != null)
            {
                return owner;
            }
            throw new NoRestaurantOwnerFoundException();
        }

        public async Task<List<RestaurantOwner>> GetAsync()
        {
            var owners = _context.RestaurantOwners.ToList();
            LogInformation("Restaurant Owners retrieved successfully.");
            return owners;
        }

        public async Task<RestaurantOwner> Update(RestaurantOwner item)
        {
            var owner = await GetAsync(item.OwnerId);
            if (owner != null)
            {
                _context.Entry<RestaurantOwner>(item).State = EntityState.Modified;
                _context.SaveChanges();
                LogInformation($"Restaurant Owner Updated: {item.OwnerId}");
                return owner;
            }
            throw new NoRestaurantOwnerFoundException();
        }

        public async Task<RestaurantOwner> GetAsync(string name)
        {
            var restaurantOwners = _context.RestaurantOwners.ToList();
            var restaurantOwner = restaurantOwners.FirstOrDefault(r => r.Name == name);
            if (restaurantOwner != null)
            {
                return restaurantOwner;
            }
            throw new NoRestaurantOwnerFoundException("No Restaurant Owner Found");
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }
    }

}
