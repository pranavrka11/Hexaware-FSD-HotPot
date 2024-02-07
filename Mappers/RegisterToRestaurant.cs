using HotPot.Models;
using HotPot.Models.DTO;

namespace HotPot.Mappers
{
    public class RegisterToRestaurant
    {
        RestaurantOwner restaurantOwner;
        public RegisterToRestaurant(RegisterRestaurantDTO registerRestaurant)
        {
            restaurantOwner = new RestaurantOwner();
            restaurantOwner.Name = registerRestaurant.Name;
            restaurantOwner.UserName = registerRestaurant.UserName;
        }
        public RestaurantOwner GetRestaurantOwner()
        {
            return restaurantOwner;
        }
    }
}
