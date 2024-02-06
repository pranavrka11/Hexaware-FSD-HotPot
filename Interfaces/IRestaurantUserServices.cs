using HotPot.Models;

namespace HotPot.Interfaces
{
    public interface IRestaurantUserServices
    {
        public Task<Restaurant> AddRestaurant(Restaurant restaurant);
        public Task<List<Restaurant>> GetRestaurantsByCity(string city);
    }
}
