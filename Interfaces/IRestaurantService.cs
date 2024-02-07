using HotPot.Models.DTO;

namespace HotPot.Interfaces
{
    public interface IRestaurantService
    {
        public Task<LoginUserDTO> RegisterRestaurant(RegisterRestaurantDTO registerRestaurant);
        public Task<LoginUserDTO> LogInRestaurant(LoginUserDTO loginRestaurant);
    }
}
