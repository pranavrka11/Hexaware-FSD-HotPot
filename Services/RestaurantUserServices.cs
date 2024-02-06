using HotPot.Exceptions;
using HotPot.Interfaces;
using HotPot.Models;

namespace HotPot.Services
{
    public class RestaurantUserServices : IRestaurantUserServices
    {
        private readonly IRepository<int, String, Restaurant> _restaurantRepo;
        private readonly IRepository<int, String, City> _cityRepo;
        private ILogger<RestaurantUserServices> _logger;

        public RestaurantUserServices(IRepository<int, String, Restaurant> restaurantRepo,
                                      IRepository<int, String, City> cityRepo,
                                      ILogger<RestaurantUserServices> logger)
        {
            _restaurantRepo = restaurantRepo;
            _cityRepo = cityRepo;
            _logger = logger;
        }

        public async Task<Restaurant> AddRestaurant(Restaurant restaurant)
        {
            restaurant=await _restaurantRepo.Add(restaurant);
            return restaurant;
        }

        public async Task<List<Restaurant>> GetRestaurantsByCity(string city)
        {
            var myCity = await _cityRepo.GetAsync(city);
            if(myCity!=null)
            {
                var restaurants = await _restaurantRepo.GetAsync();
                var myCityRestaurants = restaurants.Where(r => r.CityId == myCity.CityId).ToList();
                return myCityRestaurants;
            }
            throw new CityNotFoundException("City not found");
        }
    }
}
