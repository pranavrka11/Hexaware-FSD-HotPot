using HotPot.Exceptions;
using HotPot.Interfaces;
using HotPot.Models;
using HotPot.Models.DTO;
using HotPot.Repositories;
using System.Numerics;

namespace HotPot.Services
{
    public class CustomerUserService : ICustomerUserService
    {
        private readonly IRepository<int, string, CustomerAddress> _custAddressRepo;
        private readonly IRepository<int, string, CustomerReview> _custReviewRepo;
        private readonly IRepository<int, string, Menu> _menuRepo;
        private readonly IRepository<int, string, Restaurant> _restaurantRepo;
        private readonly IRepository<int, string, City> _cityRepo;
        private readonly IRepository<int, string, State> _stateRepo;
        private readonly IRepository<int, string, Order> _orderRepo;
        private readonly IRepository<int, string, Payment> _paymentRepo;
        private readonly IRepository<int, string, Cart> _cartRepo;
        private readonly IRepository<(int, int), string, OrderItem> _orderItemRepo;


        //private readonly IRepository<int, string, Cart> _cartRepo;
        //private readonly IRepository<int, string, CartItem> _cartItemRepo;
        private readonly ILogger<CustomerUserService> _logger;

        public CustomerUserService(
                IRepository<int, string, CustomerAddress> custAddressRepo,
                IRepository<int, string, CustomerReview> custReviewRepo,
                IRepository<int, string, Menu> menuRepo,
                IRepository<int, string, Restaurant> restaurantRepo,
                IRepository<int, string, City> cityRepo,
                IRepository<int, string, State> stateRepo,
                IRepository<int, string, Order> orderRepo,
                IRepository<int, string, Payment> paymentRepo,
                IRepository<int, string, Cart> cartRepo,
                IRepository<(int, int) ,string, OrderItem> orderItemRepo,
                //IRepository<int, string, Cart> cartRepo,
                //IRepository<int, string, CartItem> cartItemRepo,
                ILogger<CustomerUserService> logger)
        {
            _custAddressRepo = custAddressRepo;
            _custReviewRepo = custReviewRepo;
            _menuRepo = menuRepo; 
            _restaurantRepo = restaurantRepo;
            _cityRepo = cityRepo;
            _stateRepo = stateRepo;
            _orderRepo = orderRepo;
            _paymentRepo = paymentRepo;
            _cartRepo = cartRepo;
            _orderItemRepo = orderItemRepo;
            //_cartRepo = cartRepo;
            //_cartItemRepo = cartItemRepo;
            _logger = logger;
        }

        //Address Layer

        public async Task<CustomerAddress> AddCustomerAddress(CustomerAddress customerAddress)
        {
            customerAddress = await _custAddressRepo.Add(customerAddress);
            LogInformation($"Customer Address added successfully. AddressId: {customerAddress.AddressId}");
            return customerAddress;
            
        }


        public async Task<CustomerAddress> UpdateCustomerAddress(int addressId, CustomerAddressUpdateDTO addressUpdateDto)
        {
            var existingAddress = await _custAddressRepo.GetAsync(addressId);
            if (existingAddress == null)
            {
                throw new NoCustomerAddressFoundException($"Customer address with ID {addressId} not found.");
            }
            LogInformation("Existing Customer Address Details:");
            LogInformation($"House Number: {existingAddress.HouseNumber}");
            LogInformation($"Building Name: {existingAddress.BuildingName}");
            LogInformation($"Locality: {existingAddress.Locality}");
            LogInformation($"City ID: {existingAddress.CityId}");
            LogInformation($"Landmark: {existingAddress.LandMark}");

            if (!string.IsNullOrEmpty(addressUpdateDto.HouseNumber))
            {
                existingAddress.HouseNumber = addressUpdateDto.HouseNumber;
            }
            if (!string.IsNullOrEmpty(addressUpdateDto.BuildingName))
            {
                existingAddress.BuildingName = addressUpdateDto.BuildingName;
            }
            if (!string.IsNullOrEmpty(addressUpdateDto.Locality))
            {
                existingAddress.Locality = addressUpdateDto.Locality;
            }
            if (addressUpdateDto.CityId.HasValue)
            {
                existingAddress.CityId = addressUpdateDto.CityId.Value;
            }
            if (!string.IsNullOrEmpty(addressUpdateDto.LandMark))
            {
                existingAddress.LandMark = addressUpdateDto.LandMark;
            }

            var updatedAddress = await _custAddressRepo.Update(existingAddress);
            LogInformation($"Customer address updated successfully. AddressId: {updatedAddress.AddressId}");
            return updatedAddress;
        }

        public async Task<CustomerAddress> ViewCustomerAddressByCustomerId(int customerId)
        {
            var customerAddress = await _custAddressRepo.GetAsync(customerId);
            if (customerAddress == null)
            {
                throw new NoCustomerAddressFoundException($"No address found for customer with ID {customerId}");
            }
            return customerAddress;
        }


        //Review Layer

        public async Task<CustomerReview> AddCustomerReview(CustomerReview customerReview)
        {
            customerReview = await _custReviewRepo.Add(customerReview);
            LogInformation($"Customer Review added successfully. ReviewId: {customerReview.ReviewId}");
            return customerReview;
        }

        public async Task<CustomerReview> ViewCustomerReview(int customerReviewId)
        {
            var customerReview = await _custReviewRepo.GetAsync(customerReviewId);
            if (customerReview == null)
            {
                throw new NoCustomerReviewFoundException($"No review found with ID {customerReviewId}");
            }
            return customerReview;
        }

        public async Task<CustomerReview> UpdateCustomerReviewText(CustomerReviewUpdateDTO reviewUpdateDTO)
        {
            var existingReview = await _custReviewRepo.GetAsync(reviewUpdateDTO.ReviewId);
            if (existingReview == null)
            {
                throw new NoCustomerReviewFoundException($"Customer review with ID {reviewUpdateDTO.ReviewId} not found.");
            }
            LogInformation($"Existing Text Review: {existingReview.TextReview}");
            existingReview.TextReview = reviewUpdateDTO.TextReview;
            var updatedReview = await _custReviewRepo.Update(existingReview);
            LogInformation($"Text Review updated successfully. ReviewId: {updatedReview.ReviewId}");
            return updatedReview;
        }

        public async Task<CustomerReview> DeleteCustomerReview(int reviewId)
        {
            var existingReview = await _custReviewRepo.GetAsync(reviewId);
            if (existingReview == null)
            {
                throw new NoCustomerReviewFoundException($"Customer review with ID {reviewId} not found.");
            }
            existingReview.IsDeleted = true;
            var updatedReview = await _custReviewRepo.Update(existingReview);
            LogInformation($"Customer Review soft deleted successfully. ReviewId: {updatedReview.ReviewId}");
            return updatedReview;
        }



        //Menu Layer
        public async Task<List<Menu>> ViewMenu()
        {
            var menuItems = await _menuRepo.GetAsync();
            return menuItems;
        }

        public async Task<List<Menu>> SearchMenu(string query)
        {
            var allMenuItems = await _menuRepo.GetAsync();
            if(allMenuItems == null)
            {
                throw new NoMenuFoundException("No menu item found");
            }
            var matchingMenuItems = allMenuItems.Where(menu => menu.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            return matchingMenuItems;
        }

        public async Task<List<Menu>> FilterMenuByPriceRange(float minPrice, float maxPrice)
        {
            var allMenuItems = await _menuRepo.GetAsync();
            if (allMenuItems == null)
            {
                throw new NoMenuFoundException("No menu items found");
            }
            var filteredMenuItems = allMenuItems.Where(m => m.Price >= minPrice && m.Price <= maxPrice).ToList();
            return filteredMenuItems;
        }

        public async Task<List<Menu>> FilterMenuByType(string type)
        {
            var allMenuItems = await _menuRepo.GetAsync();
            if (allMenuItems == null)
            {
                throw new NoMenuFoundException("No menu items found");
            }
            var filteredMenuItems = allMenuItems.Where(m => string.Equals(m.Type, type, StringComparison.OrdinalIgnoreCase)).ToList();
            return filteredMenuItems;
        }

        public async Task<List<Menu>> FilterMenuByRestaurant(int restaurantId)
        {
            var restaurant = await _restaurantRepo.GetAsync(restaurantId);
            if (restaurant == null)
            {
                throw new RestaurantNotFoundException($"Restaurant with ID {restaurantId} not found.");
            }
            var allMenuItems = await _menuRepo.GetAsync();
            var filteredMenuItems = allMenuItems.Where(m => m.RestaurantId == restaurantId).ToList();
            return filteredMenuItems;
        }

        public async Task<List<Menu>> FilterMenuByCuisine(string cuisine)
        {
            var allMenuItems = await _menuRepo.GetAsync();
            if (allMenuItems == null)
            {
                throw new NoMenuFoundException("No menu with given cuisine");
            }
            var filteredMenuItems = allMenuItems.Where(m => string.Equals(m.Cuisine,cuisine, StringComparison.OrdinalIgnoreCase)).ToList();
            return filteredMenuItems;
        }

        public async Task<Menu> AddMenu(Menu menu)
        {
            try
            {
                var addedMenu = await _menuRepo.Add(menu);
                _logger.LogInformation($"Menu added successfully. MenuId: {addedMenu.MenuId}");
                return addedMenu;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while adding the menu.");
                throw;
            }
        }

        public async Task<Menu> RemoveMenu(int menuId)
        {
            try{
                var deletedMenu = await _menuRepo.Delete(menuId);
                if (deletedMenu != null)
                {
                    _logger.LogInformation($"Menu deleted: {deletedMenu.MenuId}");
                    return deletedMenu;
                }
                else
                {
                    _logger.LogWarning($"Menu with ID {menuId} not found.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error removing menu with ID {menuId}: {ex.Message}");
                return null;
            }
        }


        //Restaurant layer
        public async Task<List<Restaurant>> ViewRestaurants()
        {
            try
            {
                var restaurants = await _restaurantRepo.GetAsync();
                _logger.LogInformation("Retrieved list of restaurants successfully.");
                return restaurants;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching list of restaurants.");
                throw; 
            }
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        //Order Layer
        public async Task<List<Order>> ViewOrders()
        {
            try
            {
                var orders = await _orderRepo.GetAsync();
                _logger.LogInformation("Retrieved list of orders successfully.");
                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching list of orders.");
                throw;
            }

        }

        public async Task<List<Order>> GetOrderByCustomerId(int customerId)
        {
            var orders = await _orderRepo.GetAsync();
            return orders.Where(o => o.CustomerId == customerId).ToList();
        }


        public async Task<List<Payment>> ViewPayments()
        {
            try
            {
                var payments = await _paymentRepo.GetAsync();
                _logger.LogInformation("Retrieved list of payments successfully.");
                return payments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while fetching list of payments.");
                throw;

            }
        }

        public async Task<Order> UpdateOrderStatus(int orderId, string newStatus)
        {
            try
            {
                var order = await _orderRepo.GetAsync(orderId);
                if(order == null)
                {
                    throw new NoOrderFoundException();
                }

                order.Status = newStatus;
                var updatedOrder = await _orderRepo.Update(order);
                LogInformation($"Order status updated successfully. OrderId: {updatedOrder.OrderId}, New Status: {updatedOrder.Status}");
                return updatedOrder;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating order status.");
                throw;
            }
        }

        public async Task<List<Order>> CreateOrdersFromCart(int customerId)
        {
            try
            {
                // Retrieve cart items for the given customer
                var cartItems = await ViewCart(customerId);

                // Group cart items by restaurant ID
                var cartItemsByRestaurant = cartItems.GroupBy(item => item.RestaurantId);

                var orders = new List<Order>();

                // Create an order for each restaurant group
                foreach (var group in cartItemsByRestaurant)
                {
                    // Create a new order for the restaurant
                    var order = new Order
                    {
                        OrderDate = DateTime.Now,
                        CustomerId = customerId,
                        Status = "Pending", // Set initial status
                        PartnerId = 1,
                        RestaurantId = group.Key, // Restaurant ID from the group
                                                  // You can add other restaurant details to the order if needed
                                                  // Example: Restaurant = await _restaurantRepo.GetAsync(group.Key)
                        OrderItems = new List<OrderItem>() // Initialize order items collection
                    };

                    float totalAmount = 0; // Initialize total amount for the order


                    // Add order items for the current restaurant
                    foreach (var cartItem in group)
                    {
                        var orderItem = new OrderItem
                        {
                            Quantity = cartItem.Quantity,
                            SubTotalPrice = cartItem.Quantity * cartItem.Menu.Price,
                            MenuId = cartItem.MenuItemId
                        };

                        order.OrderItems.Add(orderItem);

                        // Calculate subtotal price and add to total amount
                        totalAmount += orderItem.SubTotalPrice;
                    }
                    // Set the total amount for the order
                    order.Amount = totalAmount;

                    // Add the order to the list
                    orders.Add(order);
                }

                // Save orders to the database
                foreach (var order in orders)
                {
                    await _orderRepo.Add(order);
                }

                // Remove cart items from the database (assuming you have a method to remove cart items)
                foreach (var cartItem in cartItems)
                {
                    await _cartRepo.Delete(cartItem.Id);
                }

                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while creating orders from cart for customer with ID {customerId}");
                throw new NoOrderFoundException("Error occurred while creating orders from cart. Please try again later.", ex);
            }
        }


        //cart layer
        public async Task<List<Cart>> ViewCart(int customerId)
        {
            try
            {
                var carts = await _cartRepo.GetAsync();
                var customerCarts = carts.Where(c => c.CustomerId == customerId).ToList();
                return customerCarts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching cart for customer with ID {customerId}");
                throw;
            }
        }
    }
}
