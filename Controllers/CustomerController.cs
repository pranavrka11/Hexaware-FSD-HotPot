using HotPot.Exceptions;
using HotPot.Models.DTO;
using HotPot.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotPot.Services;
using HotPot.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace HotPot.Controllers
{
    [EnableCors("HotpotPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerUserService _customerUserService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerUserService customerUserService, ILogger<CustomerController> logger)
        {
            _customerUserService = customerUserService;
            _logger = logger;
        }

        // Address Endpoints

        [HttpPost("address")]
        public async Task<IActionResult> AddCustomerAddress(CustomerAddress customerAddress)
        {
            try
            {
                var addedAddress = await _customerUserService.AddCustomerAddress(customerAddress);
                return Ok(addedAddress);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding customer address: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("address/{addressId}")]
        public async Task<IActionResult> UpdateCustomerAddress(int addressId, CustomerAddressUpdateDTO addressUpdateDto)
        {
            try
            {
                var updatedAddress = await _customerUserService.UpdateCustomerAddress(addressId, addressUpdateDto);
                return Ok(updatedAddress);
            }
            catch (NoCustomerAddressFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating customer address: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        //[Authorize(Roles = "RestaurantOwner")]
        [HttpGet("address/{customerId}")]
        public async Task<IActionResult> ViewCustomerAddressByCustomerId(int customerId)
        {
            try
            {
                var customerAddress = await _customerUserService.ViewCustomerAddressByCustomerId(customerId);
                return Ok(customerAddress);
            }
            catch (NoCustomerAddressFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching customer address for ID {customerId}: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        // Review Endpoints

        [HttpPost("review")]
        public async Task<IActionResult> AddCustomerReview(CustomerReview customerReview)
        {
            try
            {
                var addedReview = await _customerUserService.AddCustomerReview(customerReview);
                return Ok(addedReview);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding customer review: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("review/{customerReviewId}")]
        public async Task<IActionResult> ViewCustomerReview(int customerReviewId)
        {
            try
            {
                var review = await _customerUserService.ViewCustomerReview(customerReviewId);
                return Ok(review);
            }
            catch (NoCustomerReviewFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving customer review: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("review")]
        public async Task<IActionResult> UpdateCustomerReviewText(CustomerReviewUpdateDTO reviewUpdateDTO)
        {
            try
            {
                var updatedReview = await _customerUserService.UpdateCustomerReviewText(reviewUpdateDTO);
                return Ok(updatedReview);
            }
            catch (NoCustomerReviewFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating customer review: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("review/{reviewId}")]
        public async Task<IActionResult> DeleteCustomerReview(int reviewId)
        {
            try
            {
                var deletedReview = await _customerUserService.DeleteCustomerReview(reviewId);
                return Ok(deletedReview);
            }
            catch (NoCustomerReviewFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting customer review: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Menu Endpoints

        [HttpGet("menu")]
        public async Task<IActionResult> ViewMenu()
        {
            try
            {
                var menuItems = await _customerUserService.ViewMenu();
                return Ok(menuItems);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving menu: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("menu/search")]
        public async Task<IActionResult> SearchMenu([FromQuery] string query)
        {
            try
            {
                var matchingMenuItems = await _customerUserService.SearchMenu(query);
                return Ok(matchingMenuItems);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error searching menu: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("menu/filter/price")]
        public async Task<IActionResult> FilterMenuByPriceRange([FromQuery] float minPrice, [FromQuery] float maxPrice)
        {
            try
            {
                var filteredMenuItems = await _customerUserService.FilterMenuByPriceRange(minPrice, maxPrice);
                return Ok(filteredMenuItems);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error filtering menu by price range: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("menu/filter/type")]
        public async Task<IActionResult> FilterMenuByType([FromQuery] string type)
        {
            try
            {
                var filteredMenuItems = await _customerUserService.FilterMenuByType(type);
                return Ok(filteredMenuItems);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error filtering menu by type: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("menu/filter/restaurant")]
        public async Task<IActionResult> FilterMenuByRestaurant([FromQuery] int restaurantId)
        {
            try
            {
                var filteredMenuItems = await _customerUserService.FilterMenuByRestaurant(restaurantId);
                return Ok(filteredMenuItems);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error filtering menu by restaurant: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("menu/filter/cuisine")]
        public async Task<IActionResult> FilterMenuByCuisine([FromQuery] string cuisine)
        {
            try
            {
                var filteredMenuItems = await _customerUserService.FilterMenuByCuisine(cuisine);
                return Ok(filteredMenuItems);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error filtering menu by cuisine: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("menu/add")]
        public async Task<IActionResult> AddMenu(Menu menu)
        {
            try
            {
                var addedMenu = await _customerUserService.AddMenu(menu);
                return Ok(addedMenu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding the menu.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("menu/delete")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            try
            {
                var deletedMenu = await _customerUserService.RemoveMenu(id);
                if (deletedMenu != null)
                {
                    return Ok(deletedMenu); 
                }
                else
                {
                    return NotFound(); 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting menu with ID {id}: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request."); 
            }
        }

        //Restaurant Endpoints
        [HttpGet("restaurants")]
        public async Task<IActionResult> ViewRestaurants()
        {
            try
            {
                var restaurants = await _customerUserService.ViewRestaurants();
                return Ok(restaurants);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving restaurants: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        //Order Endpoints
        [HttpGet("orders")]
        public async Task<IActionResult> ViewOrders()
        {
            try
            {
                var orders = await _customerUserService.ViewOrders();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving orders: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("Payments")]
        public async Task<IActionResult> ViewPayments()
        {
            try
            {
                var payments = await _customerUserService.ViewPayments();
                return Ok(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving payments: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{orderId}/status/{newStatus}")]
        public async Task<IActionResult> UpdateOredrStatus(int orderId, string newStatus)
        {
            try
            {
                var updatedOrder = await _customerUserService.UpdateOrderStatus(orderId, newStatus);
                return Ok(updatedOrder);
            }
            catch (NoOrderFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch
            {
                return StatusCode(500, "An error occurred while updating the order status.");
            }
        }

        [HttpPost("create-orders/{customerId}")]
        public async Task<IActionResult> CreateOrdersFromCart(int customerId)
        {
            try
            {
                var orders = await _customerUserService.CreateOrdersFromCart(customerId);
                return Ok(orders);
            }
            catch (NoOrderFoundException ex)
            {
                _logger.LogError(ex, $"Error occurred while creating orders from cart for customer with ID {customerId}");
                return StatusCode(500, "Error occurred while creating orders from cart. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error occurred while creating orders from cart for customer with ID {customerId}");
                return StatusCode(500, "Unexpected error occurred. Please try again later.");
            }
        }

        //Cart Endpoints
        [HttpGet("cart/{customerId}")]
        public async Task<IActionResult> ViewCart(int customerId)
        {
            try
            {
                var carts = await _customerUserService.ViewCart(customerId);
                return Ok(carts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching cart for customer with ID {customerId}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
