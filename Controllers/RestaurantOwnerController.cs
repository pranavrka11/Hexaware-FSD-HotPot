using HotPot.Interfaces;
using HotPot.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotPot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantOwnerController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly ILogger<RestaurantOwnerController> _logger;

        public RestaurantOwnerController(IRestaurantService restaurantService , ILogger<RestaurantOwnerController> logger)
        { 
            _restaurantService = restaurantService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<LoginUserDTO>> RegisterRestaurant(RegisterRestaurantDTO registerRestaurant)
        {
            try
            {
                var result = await _restaurantService.RegisterRestaurant(registerRestaurant);
                _logger.LogInformation($"Restaurant registered successfully: {registerRestaurant.Name}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error registering restaurant: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error registering restaurant");
            }
        }

        [Route("LoginResto")]
        [HttpPost]
        public async Task<ActionResult<LoginUserDTO>> LogInRestaurant(LoginUserDTO loginRestaurant)
        {
            try
            {
                var result = await _restaurantService.LogInRestaurant(loginRestaurant);
                _logger.LogInformation($"Restaurant logged in successfully: {loginRestaurant.UserName}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error logging in restaurant: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error logging in restaurant");
            }
        }

    }
}
