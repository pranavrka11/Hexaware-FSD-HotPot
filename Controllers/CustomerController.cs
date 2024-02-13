using HotPot.Exceptions;
using HotPot.Interfaces;
using HotPot.Models;
using HotPot.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotPot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerServices _services;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerServices services, ILogger<CustomerController> logger)
        {
            _services = services;
            _logger = logger;
        }

        [Route("LogIn")]
        [HttpPost]
        public async Task<ActionResult<LoginUserDTO>> CustomerLogin(LoginUserDTO loginUser)
        {
            try
            {
                loginUser = await _services.LogIn(loginUser);
                return loginUser;
            }
            catch(InvalidUserException e)
            {
                _logger.LogCritical(e.Message);
                return Unauthorized("Invalid username or password");
            }
        }

        [Route("Register")]
        [HttpPost]
        public async Task<LoginUserDTO> CustomerRegistration(RegisterCustomerDTO registerCustomer)
        {
            var result = await _services.RegisterCustomer(registerCustomer);
            return result;
        }

        //[Authorize(Roles ="Customer")]
        [Route("GetRestaurantsByCity")]
        [HttpGet]
        public async Task<ActionResult<List<Restaurant>>> GetRestaurantsByCity(string city)
        {
            try
            {
                var result = await _services.GetRestaurantsByCity(city);
                return result;
            }
            catch(CityNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound(e.Message);
            }
            catch (RestaurantNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound(e.Message);
            }
        }

        [Route("GetRestaurantByName")]
        [HttpGet]
        public async Task<ActionResult<Restaurant>> GetRestaurantByName(string name)
        {
            try
            {
                var restaurant = await _services.GetRestaurantByName(name);
                return restaurant;
            }
            catch (RestaurantNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound("Can't find the restaurant you're looking for");
            }
        }

        [Route("GetMenuByRestaurant")]
        [HttpGet]
        public async Task<ActionResult<List<Menu>>> GetMenuByRestaurant(int restaurantId)
        {
            try
            {
                var result = await _services.GetMenuByRestaurant(restaurantId);
                return result;
            }
            catch (NoMenuAvailableException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound("No Menu available to show at the moment");
            }
        }

        [Route("AddToCart")]
        [HttpPost]
        public async Task<CartMenuDTO> AddToCart(int userId, int menuItem)
        {
            var cart=await _services.AddToCart(userId, menuItem);
            return cart;
        }

        [Route("ViewCart")]
        [HttpGet]
        public async Task<ActionResult<List<CartMenuDTO>>> GetCarts(int userId)
        {
            try
            {
                var carts = await _services.GetCarts(userId);
                return carts;
            }
            catch(EmptyCartException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound("Cart is empty");
            }
        }

        [Route("IncreaseCartItemQuantity")]
        [HttpPut]
        public Task<Cart> IncreaseCartItemQuantity(int cartId)
        {
            var cart=_services.IncreaseCartItemQuantity(cartId);
            return cart;
        }

        [Route("DecreaseCartItemQuantity")]
        [HttpPut]
        public Task<Cart> DecreaseCartItemQuantity(int cartId)
        {
            var cart = _services.DecreaseCartItemQuantity(cartId);
            return cart;
        }

        [Route("PlaceOrderForOne")]
        [HttpPost]
        public Task<OrderMenuDTO> PlaceOrderForOne(int cartId, string paymentMode)
        {
            var order = _services.PlaceOrderForOne(cartId, paymentMode);
            return order;
        }

        [Route("PlaceOrderForAll")]
        [HttpPost]
        public Task<OrderMenuDTO> PlaceOrderForAll(int customerId,  string paymentMode)
        {
            var order = _services.PlaceOrder(customerId, paymentMode);
            return order;
        }

        [Route("ViewOrderStatus")]
        [HttpGet]
        public async Task<ActionResult<OrderMenuDTO>> ViewOrderStatus(int orderId)
        {
            try
            {
                var orderStatus = await _services.ViewOrderStatus(orderId);
                return orderStatus;
            }
            catch(OrdersNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound(e.Message);
            }
        }
    }
}
